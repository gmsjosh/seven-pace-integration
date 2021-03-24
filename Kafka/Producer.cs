using Chr.Avro.Confluent;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Chr.Avro.Resolution;
using Chr.Avro.Abstract;

namespace seven_pace_kafka
{
    public class Producer<TKey, TValue>
    {
        private ProducerConfig producerConfig = new ProducerConfig();
        private SchemaRegistryConfig registryConfig = new SchemaRegistryConfig();
        private string topic;
        private Queue<KeyValuePair<TKey, TValue>> KVPS = new Queue<KeyValuePair<TKey, TValue>>();
        public Producer(string broker, string schemaRegistryURL, string topic)
        {
            producerConfig.BootstrapServers = broker;
            registryConfig.Url = schemaRegistryURL;
            this.topic = topic;
            new Thread(Start).Start();
        }
        private void Start()
        {
            using (var registry = new CachedSchemaRegistryClient(registryConfig))
            {
                var typeResolver = new ReflectionResolver(resolveReferenceTypesAsNullable: true);
                var schemaBuilder = new SchemaBuilder(typeResolver: typeResolver);
                using (var serializerBuilder = new SchemaRegistrySerializerBuilder(registry, schemaBuilder: schemaBuilder))
                {
                    var builder = new ProducerBuilder<TKey, TValue>(producerConfig);
                    Task t = builder.SetAvroValueSerializer(serializerBuilder, this.topic, registerAutomatically: AutomaticRegistrationBehavior.Always);
                    while (!t.IsCompletedSuccessfully) ;
                    using (var producer = builder.Build())
                    {
                        while (true)
                        {
                            if (KVPS.Count == 0) Thread.Sleep(1);
                            else
                            {
                                lock (KVPS)
                                {
                                    foreach (KeyValuePair<TKey, TValue> KVP in KVPS)
                                    {
                                        try
                                        {
                                            Task p = producer.ProduceAsync(topic, new Message<TKey, TValue>
                                            {
                                                Key = KVP.Key,
                                                Value = KVP.Value
                                            });
                                            while (!t.IsCompleted) Console.WriteLine(p.Status);
                                            Console.WriteLine(p.Status);
                                        }
                                        catch (Exception e)
                                        {
                                            System.Console.WriteLine(e);
                                        }
                                    }
                                    KVPS.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SendMessage(TKey key, TValue value)
        {
            lock (KVPS)
                KVPS.Enqueue(CreateKVP(key, value));
            /* using (var registry = new CachedSchemaRegistryClient(registryConfig))
            {
                var typeResolver = new ReflectionResolver(resolveReferenceTypesAsNullable: true);
                var schemaBuilder = new SchemaBuilder(typeResolver: typeResolver);
                var serializerBuilder = new SchemaRegistrySerializerBuilder(registry, schemaBuilder: schemaBuilder);
                var builder = new ProducerBuilder<TKey, TValue>(producerConfig);
                await builder.SetAvroValueSerializer(serializerBuilder, this.topic,  registerAutomatically: AutomaticRegistrationBehavior.Always);
                
                using (var producer = builder.Build())
                {
                    await producer.ProduceAsync(topic, new Message<TKey, TValue>
                    {
                        Key = key,
                        Value = value
                    });

                }
            } */
        }
        private KeyValuePair<TKey, TValue> CreateKVP(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}