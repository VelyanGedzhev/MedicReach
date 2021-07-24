using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.Physicians;
using MedicReach.Services.Physicians;
using MedicReach.Services.Physicians.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Controllers
{
    public class PhysiciansController : Controller
    {
        private readonly IPhysicianService physicians;
        private readonly MedicReachDbContext data;

        public PhysiciansController(MedicReachDbContext data, IPhysicianService physicians)
        {
            this.data = data;
            this.physicians = physicians;
        }

        public IActionResult All([FromQuery]AllPhysiciansQueryModel query)
        {
            var queryResult = this.physicians.All(
                query.Speciality,
                query.MedicalCenter,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllPhysiciansQueryModel.PhysiciansPerPage);

            var physicianSpecialities = this.physicians.AllSpecialities();

            var medicalCenters = this.physicians.AllMedicalCenters();

            query.MedicalCenters = medicalCenters;
            query.Specialities = physicianSpecialities;
            query.Physicians = queryResult.Physicians;
            query.TotalPhysicians = queryResult.TotalPhysicians;

            return View(query);
        }

        public IActionResult Add() => View(new BecomePhysicianFormModel
        {
            MedicalCenters = this.GetMedicalCenters(),
            Specialities = this.GetSpecialities()
        });

        [HttpPost]
        public IActionResult Add(BecomePhysicianFormModel physicianModel)
        {
            if (!this.data.Addresses.Any(a => a.Id == physicianModel.MedicalCenterId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.MedicalCenterId), "MedicalCenter does not exist.");
            }

            if (!this.data.PhysicianSpecialities.Any(ps => ps.Id == physicianModel.SpecialityId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.SpecialityId), "Speciality does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                physicianModel.MedicalCenters = this.GetMedicalCenters();
                physicianModel.Specialities = this.GetSpecialities();

                return View(physicianModel);
            }

            var physicianImageUrl = string.Empty;

            if (physicianModel.Gender == GenderMale)
            {
                physicianImageUrl = DefaultMaleImageUrl;
            }
            else
            {
                physicianImageUrl = DefaultFemaleImageUrl;
            }

            var physician = new Physician
            {
                FirstName = physicianModel.FirstName,
                LastName = physicianModel.LastName,
                Gender = physicianModel.Gender,
                Email = physicianModel.Email,
                ExaminationPrice = physicianModel.ExaminationPrice,
                MedicalCenterId = physicianModel.MedicalCenterId,
                ImageUrl = physicianModel.ImageUrl ?? physicianImageUrl,
                SpecialityId = physicianModel.SpecialityId,
                IsWorkingWithChildren = physicianModel.IsWorkingWithChildren
            };

            this.data.Physicians.Add(physician);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int physicianId)
        {
            var physician = this.data
                .Physicians
                .Where(p => p.Id == physicianId)
                .Select(p => new PhysicianServiceModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    ExaminationPrice = p.ExaminationPrice,
                    Speciality = p.Speciality.Name,
                    ImageUrl = p.ImageUrl,
                    Address = $"{p.MedicalCenter.Address.Number} {p.MedicalCenter.Address.Name}, {p.MedicalCenter.Address.City}, {p.MedicalCenter.Address.Country.Alpha3Code}",
                    IsWorkingWithChildren = p.IsWorkingWithChildren == true ? "Yes" : "No",
                    MedicalCenter = p.MedicalCenter
                })
                .FirstOrDefault();

            return View(physician);
        }

        private IEnumerable<PhysicianMedicalCentersViewModel> GetMedicalCenters()
            => this.data
                .MedicalCenters
                .Select(mc => new PhysicianMedicalCentersViewModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Address = mc.Address.Name,
                    AddressNumber = mc.Address.Number,
                    City = mc.Address.City,
                    CountryCoude = mc.Address.Country.Alpha3Code
                })
                .ToList();

        private IEnumerable<PhysicianSpecialityViewModel> GetSpecialities()
            => this.data
                .PhysicianSpecialities
                .Select(ps => new PhysicianSpecialityViewModel
                {
                    Id = ps.Id,
                    Name = ps.Name
                })
                .ToList();
    }
}
