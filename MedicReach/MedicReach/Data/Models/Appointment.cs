using MedicReach.Data.Models.Enums;
using System;

namespace MedicReach.Data.Models
{
    public class Appointment
    {
        public int Id { get; init; }

        public int PhysicianId { get; init; }

        public Physician Physician { get; init; }

        public int PatientId { get; init; } = 1;

        public DateTime? Date { get; set; }

        public bool IsAvailable { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}
