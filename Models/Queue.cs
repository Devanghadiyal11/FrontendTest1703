using System;

namespace Frontendtest1703.Models
{
    public class QueueEntry
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int TokenNumber { get; set; }
        public string Status { get; set; } = "waiting"; 
        public DateTime Date { get; set; }

        public Appointment? Appointment { get; set; }
        public string? PatientName { get; set; }
    }
}
