using System.Collections.Generic;

namespace Frontendtest1703.Models
{
    public class DashboardViewModel
    {
        public string Role { get; set; } = null!;

        // Admin Data
        public List<User>? Users { get; set; }
        public Clinic? ClinicInfo { get; set; }

        // Doctor and Receptionist Data
        public List<QueueEntry>? Queue { get; set; }

        // Patient Data
        public List<Appointment>? Appointments { get; set; }
        public List<Prescription>? Prescriptions { get; set; }
        public List<Report>? Reports { get; set; }
    }
}
