using System;

namespace MedicReach.Services.Appointments.Models
{
    public class AppointmentServiceModel
    {
        public string PhysicianId { get; init; }

        public string PhysicianName { get; init; }

        public string PatientId { get; init; }

        public string PatientName { get; init; }

        public DateTime Date { get; set; }
    }
}
