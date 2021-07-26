using System;

namespace MedicReach.Data.Models
{
    public class Review
    {
        public int Id { get; init; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int PhysicianId { get; set; }

        public Physician Physician { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
