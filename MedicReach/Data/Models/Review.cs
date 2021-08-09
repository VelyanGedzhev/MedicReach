using System;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Data.Models
{
    public class Review
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string PatientId { get; init; }

        public Patient Patient { get; set; }

        [Required]
        public string PhysicianId { get; set; }

        public Physician Physician { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
