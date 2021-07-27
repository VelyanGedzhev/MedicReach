using MedicReach.Data.Models;

namespace MedicReach.Services.Physicians.Models
{
    public class PhysicianServiceModel
    {
        public int Id { get; init; }

        public string FullName { get; init; }

        public string Gender { get; init; }

        public int MedicalCenterId { get; init; }

        public MedicalCenter MedicalCenter { get; init; }

        public int ExaminationPrice { get; init; }

        public string ImageUrl { get; init; }

        public string Address { get; init; }

        public int SpecialityId { get; init; }

        public string Speciality { get; init; }

        public string IsWorkingWithChildren { get; init; }
    }
}
