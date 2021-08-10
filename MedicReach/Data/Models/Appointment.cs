using System;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Data.Models
{
    public class Appointment
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string PhysicianId { get; init; }

        public Physician Physician { get; init; }

        [Required]
        public string PatientId { get; init; }

        public Patient Patient { get; set; }

        public DateTime Date { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReviewed { get; set; }
    }
}
