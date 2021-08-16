using MedicReach.Data.Models;

namespace MedicReach.Tests.Data
{
    public class Cities
    {
        public static City GetCity(
            string name,
            int countryId)
        {
            var city = new City
            {
                Name = name,
                CountryId = countryId,
                Country = new Country { Id = countryId}
                
            };

            return city;
        }
    }
}
