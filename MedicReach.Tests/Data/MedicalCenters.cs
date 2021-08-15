using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public static class MedicalCenters
    {
        public static IEnumerable<MedicalCenter> GetMedicalCenters
            => Enumerable.Range(0, 10).Select(mc => new MedicalCenter 
            {
                Physicians = new List<Physician> { new Physician { IsApproved = true} },
                Country = new Country { Name = "Bulgaria"},
                City = new City { Name = "Sofia", CountryId = 1},
                Type = new MedicalCenterType { Name = "Hospital"}
            });

        public static MedicalCenter GetMedicalCenter(string medicalCenterId, string joiningCode)
        {
            return new MedicalCenter
            {
                Id = medicalCenterId,
                Physicians = new List<Physician> { new Physician { IsApproved = true } },
                Country = new Country { Name = "Bulgaria" },
                City = new City { Name = "Sofia", CountryId = 1 },
                Type = new MedicalCenterType { Name = "Hospital" },
                JoiningCode = joiningCode
            };
        }
    }
}
