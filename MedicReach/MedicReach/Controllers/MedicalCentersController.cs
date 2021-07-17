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
                    Address = $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City}",
                    Description = mc.Description,
                    ImageUrl = mc.ImageUrl
                })
                .ToList();

            return View(medicalCenters);
        }


        public IActionResult Add() => View(new AddMedicalCenterFormModel
        {
            Addresses = GetAddresses()
        });

        [HttpPost]
        public IActionResult Add(AddMedicalCenterFormModel medicalCenter)
        {
            if (!this.data.Addresses.Any(a => a.Id == medicalCenter.AddressId))
            {
                this.ModelState.AddModelError(nameof(medicalCenter.AddressId), "Address does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenter.Addresses = this.GetAddresses();

                return View(medicalCenter);
            }

            var medicalCenterToAdd = new MedicalCenter
            {
                Name = medicalCenter.Name,
                AddressId = medicalCenter.AddressId,
                Description = medicalCenter.Description,
                ImageUrl = medicalCenter.ImageUrl ?? DefaultImageUrl
            };

            this.data.MedicalCenters.Add(medicalCenterToAdd);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<MedicalCenterAddressViewModel> GetAddresses()
            => this.data
                .Addresses
                .Select(c => new MedicalCenterAddressViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Number = c.Number,
                    City = c.City
                })
                .ToList();
    }
}
