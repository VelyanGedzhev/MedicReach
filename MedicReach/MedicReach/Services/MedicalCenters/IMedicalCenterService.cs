using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;

namespace MedicReach.Services.MedicalCenters
{
    public interface IMedicalCenterService
    {
        MedicalCenterQueryServiceModel All(
            string Type,
            string Country,
            string searchTerm,
            MedicalCentersSorting sorting,
            int currentPage,
            int physiciansPerPage);

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


        MedicalCenterServiceModel Details(int medicalCenterId);

        IEnumerable<string> AllCountries();

        IEnumerable<string> AllTypes();

        IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes();

        IEnumerable<MedicalCenterAddressServiceModel> GetAddresses();

        bool MedicalCenterTypeExists(int typeId);

        bool MedicalCenterAddressExists(int addressId);
            
    }
}
