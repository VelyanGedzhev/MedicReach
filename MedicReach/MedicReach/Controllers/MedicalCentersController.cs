using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.MedicalCenters;
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

        public IActionResult All()
        {
            var medicalCenters = this.data
                .MedicalCenters
                .Select(mc => new MedicalCenterListingViewModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Address = $"{mc.Address.Number} {mc.Address.Name}, {mc.Address.City}, {mc.Address.Country.Alpha3Code}",
                    Type = mc.MedicalCenterType.Name,
                    Description = mc.Description,
                    ImageUrl = mc.ImageUrl
                })
                .ToList();

            return View(medicalCenters);
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
