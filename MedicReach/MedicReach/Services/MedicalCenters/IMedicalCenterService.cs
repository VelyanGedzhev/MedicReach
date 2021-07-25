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

        IEnumerable<string> AllCountries();

        IEnumerable<string> AllTypes();

        IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes();

        IEnumerable<MedicalCenterAddressServiceModel> GetAddresses();
    }
}
