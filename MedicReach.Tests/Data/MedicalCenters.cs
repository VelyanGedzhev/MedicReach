using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public static class MedicalCenters
    {
        public static IEnumerable<MedicalCenter> TenApprovedMedicalCenters
            => Enumerable.Range(0, 10).Select(mc => new MedicalCenter 
            {
                Physicians = new List<Physician> { new Physician { IsApproved = true} },
                Country = new Country { Name = "Bulgaria"},
                City = new City { Name = "Sofia", CountryId = 1},
                Type = new MedicalCenterType { Name = "Hospital"}
            });

        public static MedicalCenter GetMedicalCenter(string medicalCenterId = null, string joiningCode = null, string creatorId = null) 
            => new()
            {
                Id = medicalCenterId,
                Physicians = new List<Physician> { new Physician { IsApproved = true } },
                Country = new Country { Name = "Bulgaria" },
                City = new City { Name = "Sofia", CountryId = 1 },
                Type = new MedicalCenterType { Name = "Hospital" },
                JoiningCode = joiningCode,
                CreatorId = creatorId
            };

        public static IEnumerable<MedicalCenter> GetMedicalCenters(string medicalCenterId = null, string joiningCode = null, string creatorId = null)
        {
            var medicalCenter =  new MedicalCenter
            {
                Id = medicalCenterId,
                Physicians = new List<Physician> { new Physician {UserId = creatorId, IsApproved = true } },
                Country = new Country { Name = "Bulgaria" },
                City = new City { Name = "Sofia", CountryId = 1 },
                Type = new MedicalCenterType { Name = "Hospital" },
                JoiningCode = joiningCode,
                CreatorId = creatorId
            };

            var medicalCenters = TenApprovedMedicalCenters.ToList();
            medicalCenters.Add(medicalCenter);

            return medicalCenters;
        }
    }
}
