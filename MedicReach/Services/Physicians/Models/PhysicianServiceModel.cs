using MedicReach.Data.Models;
using MedicReach.Services.Appointments.Models;
using MedicReach.Services.Reviews.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Physicians.Models
{
    public class PhysicianServiceModel
    {
        public string Id { get; init; }

        public string FullName { get; init; }

        public string Gender { get; init; }

        public string MedicalCenterId { get; init; }

        public string JoiningCode { get; set; }

        public MedicalCenter MedicalCenter { get; init; }

        public int ExaminationPrice { get; init; }

        public string ImageUrl { get; init; }

        public string Address { get; init; }

        public int SpecialityId { get; init; }

        public string Speciality { get; init; }

        public string IsWorkingWithChildren { get; init; }

        public string PracticePermissionNumber { get; init; }

        public bool IsApproved { get; init; }

        public double AverageRating { get; set; }

        public ReviewServiceModel LastReview { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }

        public IEnumerable<AppointmentServiceModel> Appointments { get; set; }
    }
}
