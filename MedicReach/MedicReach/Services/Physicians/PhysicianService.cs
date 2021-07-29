using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.MedicalCenters;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Services.Physicians
{
    public class PhysicianService : IPhysicianService
    {
        private readonly MedicReachDbContext data;
        private readonly IMedicalCenterService medicalCenters;
        private readonly IMapper mapper;

        public PhysicianService(
            MedicReachDbContext data, 
            IMapper mapper, 
            IMedicalCenterService medicalCenters)
        {
            this.data = data;
            this.mapper = mapper;
            this.medicalCenters = medicalCenters;
        }

        public void Create(
            string gender,
            int examinationPrice,
            int medicalCenterId,
            string imageUrl,
            int specialityId,
            bool isWorkingWithChildren,
            string userId)
        {
            string defaultImage = PrepareDefaultImage(gender);

            var physician = new Physician
            {
                Gender = gender,
                ExaminationPrice = examinationPrice,
                MedicalCenterId = medicalCenterId,
                ImageUrl = imageUrl ?? defaultImage,
                SpecialityId = specialityId,
                IsWorkingWithChildren = isWorkingWithChildren,
                UserId = userId
            };

            this.data.Physicians.Add(physician);
            this.data.SaveChanges();
        }

        public void Edit(
            int id,
            string gender,
            int examinationPrice,
            int medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string UserId)
        {
            var physicanToEdit = this.data
                .Physicians
                .Find(id);

            physicanToEdit.ExaminationPrice = examinationPrice;
            physicanToEdit.MedicalCenterId = medicalCenterId;
            physicanToEdit.SpecialityId = specialityId;
            physicanToEdit.IsWorkingWithChildren = IsWorkingWithChildren;
            physicanToEdit.ImageUrl = imageUrl ?? PrepareDefaultImage(physicanToEdit.Gender);

            this.data.SaveChanges();
        }

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
                    p.User.FullName.ToLower().Contains(searchTerm.ToLower()));
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
                PhysicianSorting.NameAsc => physiciansQuery.OrderBy(p => p.User.FullName),
                PhysicianSorting.NameDesc => physiciansQuery.OrderByDescending(p => p.User.FullName),
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

        public PhysicianServiceModel Details(int physicianId)
            => this.data
                .Physicians
                .Where(p => p.Id == physicianId)
                .ProjectTo<PhysicianServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

        public IEnumerable<PhysicianSpecialityServiceModel> GetSpecialities()
            => this.data
                .PhysicianSpecialities
                .ProjectTo<PhysicianSpecialityServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<PhysicianMedicalCentersServiceModel> GetMedicalCenters()
            => this.data
                .MedicalCenters
                .ProjectTo<PhysicianMedicalCentersServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

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

        public bool SpecialityExists(int specialityId)
            => this.data.PhysicianSpecialities.Any(ps => ps.Id == specialityId);

        public bool MedicalCenterExists(int medicalCenterId)
            => this.data.MedicalCenters.Any(a => a.Id == medicalCenterId);

        public bool IsPhysician(string userId)
            => this.data
                .Physicians
                .Any(p => p.UserId == userId);

        public int GetPhysicianId(string userId)
            => this.data
                .Physicians
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

        public string PrepareDefaultImage(string gender)
            => gender == GenderMale ? DefaultMaleImageUrl : DefaultFemaleImageUrl;

        private static IEnumerable<PhysicianServiceModel> GetPhysicians(IQueryable<Physician> physicianQuery)
            => physicianQuery
                .Select(p => new PhysicianServiceModel
                {
                    Id = p.Id,
                    FullName = p.User.FullName,
                    Gender = p.Gender,
                    MedicalCenter = p.MedicalCenter,
                    Speciality = p.Speciality.Name,
                    ImageUrl = p.ImageUrl,
                    ExaminationPrice = p.ExaminationPrice,
                    IsWorkingWithChildren = p.IsWorkingWithChildren ? "Yes" : "No"
                })
                .ToList();  
    }
}
