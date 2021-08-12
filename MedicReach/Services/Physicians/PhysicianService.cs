using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Services.Physicians
{
    public class PhysicianService : IPhysicianService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public PhysicianService(
            MedicReachDbContext data,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.data = data;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public void Create(
            string fullname,
            string gender,
            int examinationPrice,
            string medicalCenterId,
            string imageUrl,
            int specialityId,
            bool isWorkingWithChildren,
            string practicePermissionNumber,
            bool isApproved,
            string userId)
        {
            string defaultImage = PrepareDefaultImage(gender);

            var physician = new Physician
            {
                FullName = fullname,
                Gender = gender,
                ExaminationPrice = examinationPrice,
                MedicalCenterId = medicalCenterId,
                ImageUrl = imageUrl ?? defaultImage,
                SpecialityId = specialityId,
                IsWorkingWithChildren = isWorkingWithChildren,
                PracticePermissionNumber = practicePermissionNumber,
                IsApproved = isApproved,
                UserId = userId
            };

            var user = this.data.Users.FirstOrDefault(u => u.Id == userId);

            Task.Run(async () =>
            {
                await userManager.AddToRoleAsync(user, WebConstants.PhysicianRoleName);
            })
                .GetAwaiter()
                .GetResult();            

            this.data.Physicians.Add(physician);
            this.data.SaveChanges();
        }

        public void Edit(
            string id,
            string fullname,
            string gender,
            int examinationPrice,
            string medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string practicePermissionNumber,
            bool isApproved,
            string UserId)
        {
            var physicanToEdit = this.data
                .Physicians
                .Find(id);

            physicanToEdit.FullName = fullname;
            physicanToEdit.ExaminationPrice = examinationPrice;
            physicanToEdit.MedicalCenterId = medicalCenterId;
            physicanToEdit.SpecialityId = specialityId;
            physicanToEdit.IsWorkingWithChildren = IsWorkingWithChildren;
            physicanToEdit.ImageUrl = imageUrl ?? PrepareDefaultImage(physicanToEdit.Gender);
            physicanToEdit.PracticePermissionNumber = practicePermissionNumber;
            physicanToEdit.IsApproved = isApproved;

            this.data.SaveChanges();
        }

        public PhysicanQueryServiceModel All(
            string speciality = null,
            string medicalCenter = null,
            string searchTerm = null,
            PhysicianSorting sorting = PhysicianSorting.DateCreated,
            int currentPage = 1,
            int physiciansPerPage = int.MaxValue,
            bool approved = true)
        {
            var physiciansQuery = this.data
                .Physicians
                .Where(p => !approved || p.IsApproved);                  

            if (!string.IsNullOrEmpty(searchTerm))
            {
                physiciansQuery = physiciansQuery
                    .Where(p =>
                    p.FullName.ToLower().Contains(searchTerm.ToLower()));
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
                PhysicianSorting.NameAsc => physiciansQuery.OrderBy(p => p.FullName),
                PhysicianSorting.NameDesc => physiciansQuery.OrderByDescending(p => p.FullName),
                PhysicianSorting.Unapproved => physiciansQuery.OrderBy(p => p.IsApproved),
                PhysicianSorting.Approved => physiciansQuery.OrderByDescending(p => p.IsApproved),
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

        public PhysicianServiceModel Details(string physicianId)
        {
            var physician =  this.data
                   .Physicians
                   .Where(p => p.Id == physicianId)
                   .ProjectTo<PhysicianServiceModel>(this.mapper.ConfigurationProvider)
                   .FirstOrDefault();

            if (physician.Reviews.Any())
            {
                physician.LastReview = physician.Reviews.OrderByDescending(r => r.CreatedOn).FirstOrDefault();
                physician.AverageRating = physician.Reviews.Average(r => r.Rating);
            }

            return physician;
        }

        public void ChangeApprovalStatus(string physicianId)
        {
            var physician = this.data.Physicians.Find(physicianId);

            physician.IsApproved = !physician.IsApproved;

            this.data.SaveChanges();
        }

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

        public bool MedicalCenterExists(string medicalCenterId)
            => this.data.MedicalCenters.Any(a => a.Id.Equals(medicalCenterId));

        public bool IsPhysician(string userId)
            => this.data
                .Physicians
                .Any(p => p.UserId == userId);
        public bool PracticePermissionNumberExists(string practicePermission)
            => this.data
                .Physicians
                .Any(p => p.PracticePermissionNumber == practicePermission);

        public string GetPracticePermissionByPhysiciandId(string physicianId)
            => this.data
                .Physicians
                .Where(p => p.Id == physicianId)
                .Select(p => p.PracticePermissionNumber)
                .FirstOrDefault();

        public string GetPhysicianId(string userId)
            => this.data
                .Physicians
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

        public string PrepareDefaultImage(string gender)
            => gender == GenderMale ? DefaultMaleImageUrl : DefaultFemaleImageUrl;

        private IEnumerable<PhysicianServiceModel> GetPhysicians(IQueryable<Physician> physicianQuery)
            => physicianQuery
                .ProjectTo<PhysicianServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
