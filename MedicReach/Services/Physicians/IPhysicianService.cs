using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Physicians
{
    public interface IPhysicianService
    {
        void Create(
            string fullname,
            string gender,
            int examinationPrice,
            string medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string practicePermissionNumber,
            bool isApproved,
            string UserId);

        void Edit(
            string id,
            string fullname,
            string gender,
            int examinationPrice,
            string medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string practicePermissionNumber,
            bool isApproved,
            string UserId);

        PhysicanQueryServiceModel All(
            string speciality = null,
            string medicalCenter = null,
            string searchTerm = null,
            PhysicianSorting sorting = PhysicianSorting.DateCreated,
            int currentPage = 1,
            int physiciansPerPage = int.MaxValue,
            bool approved = true);

        PhysicianServiceModel Details(string physicianId);

        void ChangeApprovalStatus(string physicianId);

        IEnumerable<PhysicianSpecialityServiceModel> GetSpecialities();

        IEnumerable<PhysicianMedicalCentersServiceModel> GetMedicalCenters();

        IEnumerable<string> AllMedicalCenters();

        IEnumerable<string> AllSpecialities();

        bool SpecialityExists(int specialityId);

        bool MedicalCenterExists(string medicalCenterId);

        bool IsPhysician(string userId);

        bool PracticePermissionNumberExists(string practicePermission);

        string GetPracticePermissionByPhysiciandId(string physicianId);

        string GetPhysicianId(string userId);

        string PrepareDefaultImage(string gender);
    }
}
