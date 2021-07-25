using MedicReach.Data;
using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Services.MedicalCenters
{
    public class MedicalCenterService : IMedicalCenterService
    {
        private readonly MedicReachDbContext data;

        public MedicalCenterService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public MedicalCenterQueryServiceModel All(
            string type,
            string country,
            string searchTerm,
            MedicalCentersSorting sorting,
            int currentPage,
            int medicalCentersPerPage)
        {
            var medicalCentersQuery = this.data
                .MedicalCenters
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc =>
                    mc.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(type))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc => mc.MedicalCenterType.Name == type);
            }

            if (!string.IsNullOrEmpty(country))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc => mc.Address.Country.Name == country);
            }

            medicalCentersQuery = sorting switch
            {
                MedicalCentersSorting.PhysciansCountDesc => medicalCentersQuery.OrderByDescending(mc => mc.Physicians.Count()),
                MedicalCentersSorting.PhysciansCountAsc => medicalCentersQuery.OrderBy(mc => mc.Physicians.Count()),
                MedicalCentersSorting.NameAsc => medicalCentersQuery.OrderBy(p => p.Name),
                MedicalCentersSorting.NameDesc => medicalCentersQuery.OrderByDescending(p => p.Name),
                MedicalCentersSorting.DateCreated or _ => medicalCentersQuery.OrderByDescending(p => p.Id)
            };

            var totalMedicalCenters = medicalCentersQuery.Count();

            var medicalCenters = medicalCentersQuery
                .Skip((currentPage - 1) * medicalCentersPerPage)
                .Take(medicalCentersPerPage)
                .Select(mc => new MedicalCenterServiceModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Address = $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City} {mc.Address.Country.Name}",
                    Description = mc.Description,
                    Type = mc.MedicalCenterType.Name,
                    ImageUrl = mc.ImageUrl,
                    PhysiciansCount = mc.Physicians.Count()
                })
                .ToList();

            return new MedicalCenterQueryServiceModel
            {
                TotalMedicalCenters = totalMedicalCenters,
                CurrentPage = currentPage,
                MedicalCentersPerPage = medicalCentersPerPage,
                MedicalCenters = medicalCenters
            };
        }

        public IEnumerable<string> AllCountries()
            => this.data
                .Countries
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

        public IEnumerable<string> AllTypes()
            => this.data
                .MedicalCenterTypes
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

        public IEnumerable<MedicalCenterAddressServiceModel> GetAddresses()
            => this.data
                .Addresses
                .Select(c => new MedicalCenterAddressServiceModel
                {
                    Id = c.Id,
                    AddressName = c.Name,
                    AddressNumber = c.Number,
                    City = c.City,
                    CountryCode = c.Country.Alpha3Code
                })
                .ToList();
        public IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes()
            => this.data
                .MedicalCenterTypes
                .Select(c => new MedicalCenterTypeServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
    }
}
