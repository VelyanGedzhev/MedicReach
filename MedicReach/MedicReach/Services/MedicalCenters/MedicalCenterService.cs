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
            int addressId,
            int typeId,
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
                AddressId = addressId,
                TypeId = typeId,
                Description = description,
                JoiningCode = joiningCode,
                CreatorId = creatorId,
                ImageUrl = imageUrl ?? DefaultImageUrl
            };

            this.data.MedicalCenters.Add(medicalCenterToAdd);
            this.data.SaveChanges();
        }

        public void Edit(
           int id,
           string name,
           int addressId,
           int typeId,
           string description,
           string joiningCode,
           string imageUrl)
        {
            var medicalCenterToEdit = this.data
                .MedicalCenters
                .Find(id);

            medicalCenterToEdit.Name = name;
            medicalCenterToEdit.AddressId = addressId;
            medicalCenterToEdit.TypeId = typeId;
            medicalCenterToEdit.Description = description;
            medicalCenterToEdit.JoiningCode = joiningCode;
            medicalCenterToEdit.ImageUrl = imageUrl ?? DefaultImageUrl;

            this.data.SaveChanges();
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

        public MedicalCenterServiceModel Details(int medicalCenterId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .ProjectTo<MedicalCenterServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

        public IEnumerable<MedicalCenterAddressServiceModel> GetAddresses()
            => this.data
                .Addresses
                .ProjectTo<MedicalCenterAddressServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes()
            => this.data
                .MedicalCenterTypes
                .ProjectTo<MedicalCenterTypeServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

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

        public bool MedicalCenterAddressExists(int addressId)
            => this.data.Addresses.Any(a => a.Id == addressId);

        public bool MedicalCenterTypeExists(int typeId)
            => this.data.MedicalCenterTypes.Any(a => a.Id == typeId);

        public bool IsJoiningCodeUsed(string joiningCode)
            => this.data
                .MedicalCenters
                .Any(mc => mc.JoiningCode == joiningCode);

        public bool IsJoiningCodeCorrect(string joiningCode, int medicalCenterId)
            => this.data
                .MedicalCenters
                .Any(mc => mc.JoiningCode == joiningCode && mc.Id == medicalCenterId);

        public string GetJoiningCode(int medicalCenterId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .Select(mc => mc.JoiningCode)
                .FirstOrDefault();

        public bool IsCreator(string userId, int medicalCenterId)
            => this.data
                .MedicalCenters
                .Any(mc => mc.Id == medicalCenterId && mc.CreatorId == userId);

        public int GetMedicalCenterIdByUser(string userId)
            => this.data
                .MedicalCenters
                .Where(mc => mc.CreatorId == userId)
                .Select(mc => mc.Id)
                .FirstOrDefault();
    }
}
