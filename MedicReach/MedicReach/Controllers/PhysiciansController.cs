using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Infrastructure;
using MedicReach.Models.Physicians;
using MedicReach.Services.Physicians;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var userIsPhysicians = this.physicians.IsPhysician(this.User.GetId());

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



            this.physicians.Create(
                physicianModel.FirstName,
                physicianModel.LastName,
                physicianModel.Gender,
                physicianModel.ExaminationPrice,
                physicianModel.MedicalCenterId,
                physicianModel.ImageUrl,
                physicianModel.SpecialityId,
                physicianModel.IsWorkingWithChildren,
                this.User.GetId());

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int physicianId)
        {
            var physician = this.physicians.Details(physicianId);

            return View(physician);
        }

        [Authorize]
        public IActionResult Edit()
        {
            var userId = this.User.GetId();
            var physicianId = this.physicians.GetPhysicianId(userId);

            if (physicianId == 0)
            {
                return BadRequest();
            }

            var physician = this.physicians.Details(physicianId);

            return View(new PhysicianFormModel
            {
                FirstName = physician.FirstName,
                LastName = physician.LastName,
                Gender = physician.Gender,
                ImageUrl = physician.ImageUrl,
                IsWorkingWithChildren = physician.IsWorkingWithChildren == "Yes",
                ExaminationPrice = physician.ExaminationPrice,
                MedicalCenterId = physician.MedicalCenterId,
                SpecialityId = physician.SpecialityId,
                MedicalCenters = this.physicians.GetMedicalCenters(),
                Specialities = this.physicians.GetSpecialities()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PhysicianFormModel physician)
        {
            var userId = this.User.GetId();
            var physicianId = this.physicians.GetPhysicianId(userId);

            if (physicianId == 0)
            {
                return BadRequest();
            }

            this.physicians.Edit(
                physicianId,
                physician.FirstName,
                physician.LastName,
                physician.Gender,
                physician.ExaminationPrice,
                physician.MedicalCenterId,
                physician.ImageUrl,
                physician.SpecialityId,
                physician.IsWorkingWithChildren,
                this.User.GetId());

            return RedirectToAction(nameof(All));
        }
    }
}
