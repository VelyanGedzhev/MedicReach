using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Infrastructure;
using MedicReach.Models.MedicalCenters;
using MedicReach.Services.MedicalCenters;
using MedicReach.Services.MedicalCenters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Controllers
{
    public class MedicalCentersController : Controller
    {
        private readonly IMedicalCenterService medicalCenters;
        private readonly MedicReachDbContext data;

        public MedicalCentersController(MedicReachDbContext data, IMedicalCenterService medicalCenters)
        {
            this.data = data;
            this.medicalCenters = medicalCenters;
        }

        public IActionResult All([FromQuery]AllMedicalCentersQueryModel query)
        {
            var queryResult = this.medicalCenters.All(
                query.Type,
                query.Country,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMedicalCentersQueryModel.MedicalCentersPerPage);

            var medicalCentersTypes = this.medicalCenters.AllTypes();
            var countries = this.medicalCenters.AllCountries();

            query.Countries = countries;
            query.Types = medicalCentersTypes;
            query.TotalMedicalCenters = queryResult.TotalMedicalCenters;
            query.MedicalCenters = queryResult.MedicalCenters;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View(new AddMedicalCenterFormModel
            {
                MedicalCenterTypes = GetMedicalCenterTypes(),
                Addresses = GetAddresses()
            });
        }

        [Authorize]
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
            return RedirectToAction("Become", "Physicians");
        }

        public IActionResult Details(int medicalCenterId)
        {
            var medicalCenter = this.data
                .MedicalCenters
                .Where(mc => mc.Id == medicalCenterId)
                .Select(mc => new MedicalCenterServiceModel
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

        private bool UserIsPhysician()
            => this.data
                .Physicians
                .Any(p => p.UserId == this.User.GetId());
    }
}
