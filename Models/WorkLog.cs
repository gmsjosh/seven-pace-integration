namespace seven_pace_kafka.Models
{
    public class activityType
    {
        public string id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    public class addedByUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public string uniqueName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string vstsId { get; set; }
        public string vstsCollectionId { get; set; }
    }

    public class editedByUser
    {
        public string id { get; set; }
        public string name { get; set; }
        public string uniqueName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string vstsId { get; set; }
        public string vstsCollectionId { get; set; }
    }

    public class flags
    {
        public bool isTracked { get; set; }
        public bool isManuallyEntered { get; set; }
        public bool isChanged { get; set; }
        public bool isTrackedExtended { get; set; }
        public bool isImported { get; set; }
        public bool isFromApi { get; set; }
        public bool isBillable { get; set; }
    }

    public class user
    {
        public string id { get; set; }
        public string name { get; set; }
        public string uniqueName { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string vstsId { get; set; }
        public string vstsCollectionId { get; set; }
    }

    public class WorkLog
    {
        public string id { get; set; }
        public string timestamp { get; set; }
        public int length { get; set; }
        public int? billableLength { get; set; }
        public int? workItemId { get; set; }
        public string comment { get; set; }
        public user user { get; set; }
        public addedByUser addedByUser { get; set; }
        public editedByUser editedByUser { get; set; }
        public string createdTimestamp { get; set; }
        public string editedTimestamp { get; set; }
        public activityType activityType { get; set; }
        public flags flags { get; set; }
    }
}
