using MedicReach.Data;
using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;
using System.Linq;

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

            var physicians = physiciansQuery
                .Skip((currentPage - 1) * physiciansPerPage)
                .Take(physiciansPerPage)
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
    }
}
