using Newtonsoft.Json;

namespace Frontendtest1703.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string Tests { get; set; } = null!;
        public string Remarks { get; set; } = null!;
    }

    public class AddReportDTO
    {
        [JsonProperty("diagnosis")]
        public string? Diagnosis { get; set; }

        [JsonProperty("testRecommended")]
        public string? TestRecommended { get; set; }

        [JsonProperty("remarks")]
        public string? Remarks { get; set; }
    }
}
