using Newtonsoft.Json;

namespace Frontendtest1703.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public List<MedicineDTO> Medicines { get; set; } = new List<MedicineDTO>();
        public string Notes { get; set; } = null!;
    }

    public class AddPrescriptionDTO
    {
        [JsonProperty("medicines")]
        public List<MedicineDTO> Medicines { get; set; } = new List<MedicineDTO>();

        [JsonProperty("notes")]
        public string? Notes { get; set; }
    }

    public class MedicineDTO
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("dosage")]
        public string? Dosage { get; set; }

        [JsonProperty("duration")]
        public string? Duration { get; set; }
    }
}
