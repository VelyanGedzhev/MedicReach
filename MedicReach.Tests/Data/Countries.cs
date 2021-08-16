using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public class Countries
    {
        public static IEnumerable<Country> GetCountries(string name = null, string code = null)
        {
            var countries = Enumerable.Range(0, 3).Select(p => new Country
            {
                Cities = new List<City>(),
                MedicalCenters = new List<MedicalCenter>()
            })
            .ToList();

            var country = new Country
            {
                Name = name,
                Alpha3Code = code,
                Cities = new List<City>(),
                MedicalCenters = new List<MedicalCenter>()
            };

            countries.Add(country);

            return countries;
        }

    }
}
