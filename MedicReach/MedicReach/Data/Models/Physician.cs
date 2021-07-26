﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Data.Models
{
    public class Physician
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; init; }

        public int MedicalCenterId { get; set; }

        public MedicalCenter MedicalCenter { get; set; }

        public int ExaminationPrice { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public int SpecialityId { get; set; }

        public PhysicianSpeciality Speciality { get; set; }

        public bool IsWorkingWithChildren { get; set; }


        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        //public bool IsApproved { get; set; }

        public IEnumerable<Appointment> Appointments { get; init; } = new List<Appointment>();

        public IEnumerable<Review> Reviews { get; init; } = new List<Review>();

    }
}
