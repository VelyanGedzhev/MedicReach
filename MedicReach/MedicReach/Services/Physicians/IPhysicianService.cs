using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Physicians
{
    public interface IPhysicianService
    {
        void Create(
            string gender,
            int examinationPrice,
            int medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string UserId);

        void Edit(
            int id,
            string gender,
            int examinationPrice,
            int medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string UserId);

        PhysicanQueryServiceModel All(
            string Speciality,
            string MedicalCenter,
            string searchTerm,
            PhysicianSorting sorting,
            int currentPage,
            int physiciansPerPage);

        PhysicianServiceModel Details(int physicianId);

        IEnumerable<PhysicianSpecialityServiceModel> GetSpecialities();

        IEnumerable<PhysicianMedicalCentersServiceModel> GetMedicalCenters();

        IEnumerable<string> AllMedicalCenters();

        IEnumerable<string> AllSpecialities();

        bool SpecialityExists(int specialityId);

        bool MedicalCenterExists(int medicalCenterId);

        bool IsPhysician(string userId);

        int GetPhysicianId(string userId);

        string PrepareDefaultImage(string gender);


    }
}
