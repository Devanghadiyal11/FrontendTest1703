using System;
using Newtonsoft.Json;

namespace Frontendtest1703.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int ClinicId { get; set; }
        public string? ClinicName { get; set; }
        public DateTime Date { get; set; }
        public string? TimeSlot { get; set; }
        public string? Status { get; set; } // waiting | in_progress | done | skipped
        public int? TokenNumber { get; set; }

        public Prescription? Prescription { get; set; }
        public Report? Report { get; set; }
    }
    public class BookAppointmentDTO
    {
        [JsonProperty("appointmentDate")]
        public string AppointmentDate { get; set; } = string.Empty;

        [JsonProperty("timeSlot")]
        public string? TimeSlot { get; set; }

        [JsonProperty("clinicId")]
        [JsonIgnore]
        public int ClinicId { get; set; }

        [JsonProperty("patientId")]
        [JsonIgnore]
        public int PatientId { get; set; }
    }
}
