using System;

namespace MedicReach.Services.Appointments.Models
{
    public class AppointmentServiceModel
    {
        public int PhysicianId { get; init; }

        public string PhysicianName { get; init; }

        public int PatientId { get; init; }

        public string PatientName { get; init; }

        public DateTime Date { get; set; }
    }
}
