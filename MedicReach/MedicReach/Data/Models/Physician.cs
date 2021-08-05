using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Data.Models
{
    public class Physician
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; init; }

        [Required]
        public string MedicalCenterId { get; set; }

        public MedicalCenter MedicalCenter { get; set; }

        public int ExaminationPrice { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public int SpecialityId { get; set; }

        public PhysicianSpeciality Speciality { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        [Required]
        [MaxLength(PermissionPracticeMaxLength)]
        public string PracticePermissionNumber { get; set; }

        public bool IsApproved { get; set; } = false;

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Appointment> Appointments { get; init; } = new List<Appointment>();

        public IEnumerable<Review> Reviews { get; init; } = new List<Review>();

    }
}
