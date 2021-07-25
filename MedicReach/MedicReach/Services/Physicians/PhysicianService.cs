using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Services.Physicians
{
    public class PhysicianService : IPhysicianService
    {
        private readonly MedicReachDbContext data;

        public PhysicianService(MedicReachDbContext data) 
            => this.data = data;

        public PhysicanQueryServiceModel All(
            string speciality,
            string medicalCenter,
            string searchTerm,
            PhysicianSorting sorting,
            int currentPage,
            int physiciansPerPage)
        {
            var physiciansQuery = this.data
                .Physicians
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                physiciansQuery = physiciansQuery
                    .Where(p =>
                    (p.FirstName.ToLower() + " " + p.LastName.ToLower()).Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(speciality))
            {
                physiciansQuery = physiciansQuery
                    .Where(p => p.Speciality.Name == speciality);
            }

            if (!string.IsNullOrEmpty(medicalCenter))
            {
                physiciansQuery = physiciansQuery
                    .Where(p => p.MedicalCenter.Name == medicalCenter);
            }

            physiciansQuery = sorting switch
            {
                PhysicianSorting.ExaminationPriceDesc => physiciansQuery.OrderByDescending(p => p.ExaminationPrice),
                PhysicianSorting.ExaminationPriceAsc => physiciansQuery.OrderBy(p => p.ExaminationPrice),
                PhysicianSorting.NameAsc => physiciansQuery.OrderBy(p => p.FirstName).ThenBy(p => p.LastName),
                PhysicianSorting.NameDesc => physiciansQuery.OrderByDescending(p => p.FirstName).ThenByDescending(p => p.LastName),
                PhysicianSorting.DateCreated or _ => physiciansQuery.OrderByDescending(p => p.Id)
            };

            var totalPhysicians = physiciansQuery.Count();

            var physicians = GetPhysicians(
                    physiciansQuery
                        .Skip((currentPage - 1) * physiciansPerPage)
                        .Take(physiciansPerPage));
                

            return new PhysicanQueryServiceModel
            {
                TotalPhysicians = totalPhysicians,
                CurrentPage = currentPage,
                PhysiciansPerPage = physiciansPerPage,
                Physicians = physicians
            };
        }

        public IEnumerable<string> AllSpecialities()
            => this.data
                .PhysicianSpecialities
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

        public IEnumerable<string> AllMedicalCenters()
            => this.data
                .MedicalCenters
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

        public bool IsPhysician(string userId)
            => this.data
                .Physicians
                .Any(p => p.UserId == userId);

        private static IEnumerable<PhysicianServiceModel> GetPhysicians(IQueryable<Physician> physicianQuery)
            => physicianQuery
                .Select(p => new PhysicianServiceModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    MedicalCenter = p.MedicalCenter,
                    Speciality = p.Speciality.Name,
                    ImageUrl = p.ImageUrl,
                    ExaminationPrice = p.ExaminationPrice,
                    IsWorkingWithChildren = p.IsWorkingWithChildren ? "Yes" : "No"
                })
                .ToList();

        public IEnumerable<PhysicianSpecialityServiceModel> GetSpecialities()
            => this.data
                .PhysicianSpecialities
                .Select(ps => new PhysicianSpecialityServiceModel
                {
                    Id = ps.Id,
                    Name = ps.Name
                })
                .ToList();

        public IEnumerable<PhysicianMedicalCentersServiceModel> GetMedicalCenters()
            => this.data
                .MedicalCenters
                .Select(mc => new PhysicianMedicalCentersServiceModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Address = mc.Address.Name,
                    AddressNumber = mc.Address.Number,
                    City = mc.Address.City,
                    CountryCoude = mc.Address.Country.Alpha3Code
                })
                .ToList();

        public string PrepareDefaultImage(string gender) 
            => gender == GenderMale ? DefaultMaleImageUrl : DefaultFemaleImageUrl;
    }
}
