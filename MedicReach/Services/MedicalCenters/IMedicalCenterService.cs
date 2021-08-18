using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;

namespace MedicReach.Services.MedicalCenters
{
    public interface IMedicalCenterService
    {
        void Create(
             string name,
             string address,
             int medicalCenterTypeId,
             int cityId,
             int countryId,
             string description,
             string joiningCode,
             string CreatorId,
             string imageUrl);

        void Edit(
            string id,
            string name,
            string address,
            int typeId,
            int cityId,
            int countryId,
            string description,
            string joiningCode,
            string imageUrl);

        MedicalCenterQueryServiceModel All(
            string type = null,
            string country = null,
            string searchTerm = null,
            MedicalCentersSorting sorting = MedicalCentersSorting.DateCreated,
            int currentPage = 1,
            int medicalCentersPerPage = int.MaxValue);

        MedicalCenterServiceModel Details(string medicalCenterId);

        IEnumerable<MedicalCenterServiceModel> GetMedicalCenters();

        IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes();

        IEnumerable<string> AllTypes();

        bool MedicalCenterTypeExists(int typeId);

        bool IsJoiningCodeUsed(string joiningCode);

        bool IsJoiningCodeCorrect(string joiningCode, string medicalCenterId);

        string GetJoiningCode(string medicalCenterId);

        bool IsCreatorOfMedicalCenter(string userId, string medicalCenterId);

        bool IsCreator(string userId);

        string GetMedicalCenterIdByUser(string userId);
    }
}
