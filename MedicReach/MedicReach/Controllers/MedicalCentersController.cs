using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.MedicalCenters;
using MedicReach.Models.MedicalCenters.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Controllers
{
    public class MedicalCentersController : Controller
    {
        private readonly MedicReachDbContext data;

        public MedicalCentersController(MedicReachDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery] AllMedicalCentersQueryModel allMedicalCentersQuery)
        {
            var medicalCentersQuery = this.data
                .MedicalCenters
                .AsQueryable();

            if (!string.IsNullOrEmpty(allMedicalCentersQuery.SearchTerm))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc =>
                    mc.Name.ToLower().Contains(allMedicalCentersQuery.SearchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(allMedicalCentersQuery.Type))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc => mc.MedicalCenterType.Name == allMedicalCentersQuery.Type);
            }

            if (!string.IsNullOrEmpty(allMedicalCentersQuery.Country))
            {
                medicalCentersQuery = medicalCentersQuery
                    .Where(mc => mc.Address.Country.Name == allMedicalCentersQuery.Country);
            }

            medicalCentersQuery = allMedicalCentersQuery.Sorting switch
            {
                MedicalCentersSorting.PhysciansCountDesc => medicalCentersQuery.OrderByDescending(mc => mc.Physicians.Count()),
                MedicalCentersSorting.PhysciansCountAsc => medicalCentersQuery.OrderBy(mc => mc.Physicians.Count()),
                MedicalCentersSorting.NameAsc => medicalCentersQuery.OrderBy(p => p.Name),
                MedicalCentersSorting.NameDesc => medicalCentersQuery.OrderByDescending(p => p.Name),
                MedicalCentersSorting.DateCreated or _ => medicalCentersQuery.OrderByDescending(p => p.Id)
            };

            var totalMedicalCenters = medicalCentersQuery.Count();

            var medicalCenters = medicalCentersQuery
                .Skip((allMedicalCentersQuery.CurrentPage - 1) * AllMedicalCentersQueryModel.MedicalCentersPerPage)
                .Take(AllMedicalCentersQueryModel.MedicalCentersPerPage)
                .Select(mc => new MedicalCenterListingViewModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Address = $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City} {mc.Address.Country.Name}",
                    Description = mc.Description,
                    Type = mc.MedicalCenterType.Name,
                    ImageUrl = mc.ImageUrl,
                })
                .ToList();

            var medicalCentersTypes = this.data
                .MedicalCenterTypes
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            var countries = this.data
                .Countries
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            allMedicalCentersQuery.MedicalCenters = medicalCenters;
            allMedicalCentersQuery.Countries = countries;
            allMedicalCentersQuery.Types = medicalCentersTypes;
            allMedicalCentersQuery.TotalMedicalCenters = totalMedicalCenters;

            return View(allMedicalCentersQuery);
        }


        public IActionResult Add() => View(new AddMedicalCenterFormModel
        {
            MedicalCenterTypes = GetMedicalCenterTypes(),
            Addresses = GetAddresses()
        });

        [HttpPost]
        public IActionResult Add(AddMedicalCenterFormModel medicalCenter)
        {
            if (!this.data.Addresses.Any(a => a.Id == medicalCenter.AddressId))
            {
                this.ModelState.AddModelError(nameof(medicalCenter.AddressId), "Address does not exist.");
            }

            if (!this.data.MedicalCenterTypes.Any(a => a.Id == medicalCenter.TypeId))
            {
                this.ModelState.AddModelError(nameof(medicalCenter.AddressId), "Medical Center Type does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenter.Addresses = this.GetAddresses();
                medicalCenter.MedicalCenterTypes = this.GetMedicalCenterTypes();

                return View(medicalCenter);
            }

            var medicalCenterToAdd = new MedicalCenter
            {
                Name = medicalCenter.Name,
                AddressId = medicalCenter.AddressId,
                MedicalCenterTypeId = medicalCenter.TypeId,
                Description = medicalCenter.Description,
                ImageUrl = medicalCenter.ImageUrl ?? DefaultImageUrl
            };

            this.data.MedicalCenters.Add(medicalCenterToAdd);
            this.data.SaveChanges();

            //TODO: better way to create medical center while created physician
            return Redirect("/Physicians/Add");
        }

        public IActionResult Details(int medicalCenterId)
        {
            var medicalCenter = this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .Select(mc => new MedicalCenterDetailsViewModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Description = mc.Description,
                    Address = $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City} {mc.Address.Country.Name}",
                    Type = mc.MedicalCenterType.Name,
                    PhysiciansCount = mc.Physicians.Count(),
                    ImageUrl = mc.ImageUrl
                })
                .FirstOrDefault();

            return View(medicalCenter);
        }

        private IEnumerable<MedicalCenterTypeViewModel> GetMedicalCenterTypes()
            => this.data
                .MedicalCenterTypes
                .Select(c => new MedicalCenterTypeViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

        private IEnumerable<MedicalCenterAddressViewModel> GetAddresses()
            => this.data
                .Addresses
                .Select(c => new MedicalCenterAddressViewModel
                {
                    Id = c.Id,
                    AddressName = c.Name,
                    AddressNumber = c.Number,
                    City = c.City,
                    CountryCode = c.Country.Alpha3Code
                })
                .ToList();
    }
}
