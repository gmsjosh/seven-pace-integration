using System;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using seven_pace_kafka.Models;

namespace seven_pace_kafka
{
    class Program
    {
        private static Producer<string, WorkLog> producer = new Producer<string, WorkLog>("localhost:9092", "http://localhost:8081", "WorkLogs");
        static void Main(string[] args)
        {
            var sevenPaceWorkLogClient = new RestClient("https://gmsca.timehub.7pace.com/api/rest/workLogs/all/?api-version=3.0");
            sevenPaceWorkLogClient.Authenticator = new HttpBasicAuthenticator("OpenApi", "inserttokenhere");
            sevenPaceWorkLogClient.Timeout = -1;
            var workLogRequest = new RestRequest(Method.GET);
            IRestResponse response = sevenPaceWorkLogClient.Execute(workLogRequest);
            dynamic workLogs = JsonConvert.DeserializeObject(response.Content);
            foreach (JObject rawEntry in workLogs.data)
            {
                WorkLog entry = JsonConvert.DeserializeObject<WorkLog>(rawEntry.ToString());
                producer.SendMessage((string)entry.id, entry);
            }
        }
    }
}
