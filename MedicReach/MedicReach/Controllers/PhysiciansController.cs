using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Infrastructure;
using MedicReach.Models.Physicians;
using MedicReach.Services.Physicians;
using MedicReach.Services.Physicians.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Become()
        {
            var userIsPhysicians = this.data
                .Physicians
                .Any(p => p.UserId == this.User.GetId());

            if (userIsPhysicians)
            {
                return BadRequest();
            }

            return View(new PhysicianFormModel
            {
                MedicalCenters = this.physicians.GetMedicalCenters(),
                Specialities = this.physicians.GetSpecialities()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Become(PhysicianFormModel physicianModel)
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
                physicianModel.MedicalCenters = this.physicians.GetMedicalCenters();
                physicianModel.Specialities = this.physicians.GetSpecialities();

                return View(physicianModel);
            }

            string defaultImage = this.physicians.PrepareDefaultImage(physicianModel.Gender);

            var physician = new Physician
            {
                FirstName = physicianModel.FirstName,
                LastName = physicianModel.LastName,
                Gender = physicianModel.Gender,
                ExaminationPrice = physicianModel.ExaminationPrice,
                MedicalCenterId = physicianModel.MedicalCenterId,
                ImageUrl = physicianModel.ImageUrl ?? defaultImage,
                SpecialityId = physicianModel.SpecialityId,
                IsWorkingWithChildren = physicianModel.IsWorkingWithChildren,
                UserId = this.User.GetId()
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
    }
}
