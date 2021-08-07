using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Services.MedicalCenters
{
    public class MedicalCenterService : IMedicalCenterService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;

        public MedicalCenterService(
            MedicReachDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public void Create(
            string name,
            string address,
            int typeId,
            int cityId,
            int countryId,
            string description,
            string joiningCode,
            string creatorId,
            string imageUrl)
        {
            if (IsJoiningCodeUsed(joiningCode))
            {
                return;
            }

            var medicalCenterToAdd = new MedicalCenter
            {
                Name = name,
                Address = address,
                TypeId = typeId,
                CityId = cityId,
                CountryId = countryId,
                Description = description,
                JoiningCode = joiningCode,
                CreatorId = creatorId,
                ImageUrl = imageUrl ?? DefaultImageUrl
            };

            this.data.MedicalCenters.Add(medicalCenterToAdd);
            this.data.SaveChanges();
        }

        public void Edit(
           string id,
           string name,
           string address,
           int typeId,
           int cityId,
           int countryId,
           string description,
           string joiningCode,
           string imageUrl)
        {
            var medicalCenterToEdit = this.data
                .MedicalCenters
                .Find(id);

            medicalCenterToEdit.Name = name;
            medicalCenterToEdit.Address = address;
            medicalCenterToEdit.TypeId = typeId;
            medicalCenterToEdit.CityId = cityId;
            medicalCenterToEdit.CountryId = countryId;
            medicalCenterToEdit.Description = description;
            medicalCenterToEdit.JoiningCode = joiningCode;
            medicalCenterToEdit.ImageUrl = imageUrl ?? DefaultImageUrl;

            this.data.SaveChanges();
        }

        public MedicalCenterQueryServiceModel All(
            string type = null,
            string country = null,
            string searchTerm = null,
            MedicalCentersSorting sorting = MedicalCentersSorting.DateCreated,
            int currentPage = 1,
            int medicalCentersPerPage = int.MaxValue)
        {
            var medicalCentersQuery = this.data
                .MedicalCenters
                .Where(x => x.Physicians.Any(p => p.IsApproved))
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
                    .Where(mc => mc.Type.Name == type);
            }

            if (!string.IsNullOrEmpty(country))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc => mc.Country.Name == country);
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
                .ProjectTo<MedicalCenterServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return new MedicalCenterQueryServiceModel
            {
                TotalMedicalCenters = totalMedicalCenters,
                CurrentPage = currentPage,
                MedicalCentersPerPage = medicalCentersPerPage,
                MedicalCenters = medicalCenters
            };
        }

        public MedicalCenterServiceModel Details(string medicalCenterId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .ProjectTo<MedicalCenterServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

        public IEnumerable<MedicalCenterServiceModel> GetMedicalCenters()
            => this.data
                .MedicalCenters
                .Where(x => x.Physicians.Any(p => p.IsApproved))
                .ProjectTo<MedicalCenterServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

        public IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes()
            => this.data
                .MedicalCenterTypes
                .ProjectTo<MedicalCenterTypeServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        

        public IEnumerable<string> AllTypes()
            => this.data
                .MedicalCenterTypes
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

        public bool MedicalCenterTypeExists(int typeId)
            => this.data.MedicalCenterTypes.Any(a => a.Id == typeId);

        public bool IsJoiningCodeUsed(string joiningCode)
            => this.data
                .MedicalCenters
                .Any(mc => mc.JoiningCode == joiningCode);

        public bool IsJoiningCodeCorrect(string joiningCode, string medicalCenterId)
            => this.data
                .MedicalCenters
                .Any(mc => mc.JoiningCode == joiningCode && mc.Id == medicalCenterId);

        public string GetJoiningCode(string medicalCenterId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .Select(mc => mc.JoiningCode)
                .FirstOrDefault();

        public bool IsCreator(string userId, string medicalCenterId)
            => this.data
                .MedicalCenters
                .Any(mc => mc.Id == medicalCenterId && mc.CreatorId == userId);

        public string GetMedicalCenterIdByUser(string userId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.CreatorId == userId)
                .Select(mc => mc.Id)
                .FirstOrDefault();
    }
}
