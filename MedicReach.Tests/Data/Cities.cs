using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public class Cities
    {
        public static IEnumerable<City> GetCities()
        {
            var countries = Enumerable.Range(0, 3).Select(p => new City
            {
                CountryId = 1,
                MedicalCenters = new List<MedicalCenter>()
            })
            .ToList();

            var city = new City
            {
                MedicalCenters = new List<MedicalCenter>()
                
            };

            countries.Add(city);

            return countries;
        }

        public static City GetCity(
            string name,
            int countryId)
        {
            var city = new City
            {
                Name = name,
                CountryId = countryId,
                Country = new Country { Id = countryId},
                MedicalCenters = new List<MedicalCenter>()
            };

            return city;
        }
    }
}
