using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;

namespace MedicReach.Services.MedicalCenters
{
    public interface IMedicalCenterService
    {
        void Create(
            string name,
            int addressId,
            int medicalCenterTypeId,
            string description,
            string joiningCode,
            string CreatorId,
            string imageUrl);

        void Edit(
            int id,
            string name,
            int addressId,
            int medicalCenterTypeId,
            string description,
            string joiningCode,
            string imageUrl);

        MedicalCenterQueryServiceModel All(
            string Type,
            string Country,
            string searchTerm,
            MedicalCentersSorting sorting,
            int currentPage,
            int physiciansPerPage);

        MedicalCenterServiceModel Details(int medicalCenterId);

        IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes();

        IEnumerable<MedicalCenterAddressServiceModel> GetAddresses();

        IEnumerable<string> AllCountries();

        IEnumerable<string> AllTypes();

        bool MedicalCenterAddressExists(int addressId);

        bool MedicalCenterTypeExists(int typeId);

        bool IsJoiningCodeUsed(string joiningCode);

        bool IsJoiningCodeCorrect(string joiningCode, int medicalCenterId);

        string GetJoiningCode(int medicalCenterId);

        bool IsCreator(string userId, int medicalCenterId);

        IEnumerable<MedicalCenterServiceModel> GetMedicalCenterByUser(string userId);
    }
}
