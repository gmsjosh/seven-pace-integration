using System.ComponentModel.DataAnnotations;

namespace seven_pace_kafka
{
    public class WorkLogEntry
    {
        [Required]
        public string timestamp { get; set; }
        [Required]
        public int length { get; set; }
        [Required]
        public int workItemId { get; set; }
        [Required]
        public string email { get; set; }
    }
}
