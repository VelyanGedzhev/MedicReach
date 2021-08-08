using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Cities
{
    public interface ICityService
    {
        IEnumerable<CityServiceModel> GetCities();

        bool IsCityInCountry(int countryId, int cityId);

        IEnumerable<string> AllCities();

        void Add(string name, int countryId);

        bool IsExisting(string name);
    }
}
