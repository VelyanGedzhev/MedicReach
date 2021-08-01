using AutoMapper;
using MedicReach.Infrastructure;
using MedicReach.Models.Physicians;
using MedicReach.Services.MedicalCenters;
using MedicReach.Services.Physicians;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class PhysiciansController : Controller
    {
        private readonly IPhysicianService physicians;
        private readonly IMedicalCenterService medicalCenters;
        private readonly IMapper mapper;

        public PhysiciansController(
            IPhysicianService physicians,
            IMedicalCenterService medicalCenters,
            IMapper mapper)
        {
            this.physicians = physicians;
            this.medicalCenters = medicalCenters;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllPhysiciansQueryModel query)
        {
            var queryResult = this.physicians.All(
                query.Speciality,
                query.MedicalCenter,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllPhysiciansQueryModel.PhysiciansPerPage,
                query.Approved);

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
                return RedirectToAction(nameof(Edit));
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
            if (!this.physicians.MedicalCenterExists(physicianModel.MedicalCenterId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.MedicalCenterId), "MedicalCenter does not exist.");
            }

            if (!this.physicians.SpecialityExists(physicianModel.SpecialityId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.SpecialityId), "Speciality does not exist.");
            }

            if (!this.medicalCenters.IsJoiningCodeCorrect(physicianModel.JoiningCode, physicianModel.MedicalCenterId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.JoiningCode), "Joining code is incorrect.");
            }

            if (!this.ModelState.IsValid)
            {
                physicianModel.MedicalCenters = this.physicians.GetMedicalCenters();
                physicianModel.Specialities = this.physicians.GetSpecialities();

                return View(physicianModel);
            }

            this.physicians.Create(
                physicianModel.Gender,
                physicianModel.ExaminationPrice,
                physicianModel.MedicalCenterId,
                physicianModel.ImageUrl,
                physicianModel.SpecialityId,
                physicianModel.IsWorkingWithChildren,
                physicianModel.PracticePermissionNumber,
                physicianModel.IsApproved,
                this.User.GetId());

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int physicianId)
        {
            var physician = this.physicians.Details(physicianId);

            var physicianForm = this.mapper.Map<PhysicianFormModel>(physician);

            physicianForm.MedicalCenters = this.physicians.GetMedicalCenters();
            physicianForm.Specialities = this.physicians.GetSpecialities();
            physicianForm.JoiningCode = this.medicalCenters.GetJoiningCode(physicianForm.MedicalCenterId);

            return View(physicianForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int physicianId, PhysicianFormModel physician)
        {

            if (!this.medicalCenters.IsJoiningCodeCorrect(physician.JoiningCode, physician.MedicalCenterId))
            {
                this.ModelState.AddModelError(nameof(physician.JoiningCode), "Joining code is incorrect.");
            }

            if (!this.ModelState.IsValid)
            {
                physician.MedicalCenters = this.physicians.GetMedicalCenters();
                physician.Specialities = this.physicians.GetSpecialities();

                return View(physician);
            }

            this.physicians.Edit(
                physicianId,
                physician.Gender,
                physician.ExaminationPrice,
                physician.MedicalCenterId,
                physician.ImageUrl,
                physician.SpecialityId,
                physician.IsWorkingWithChildren,
                physician.PracticePermissionNumber,
                physician.IsApproved,
                this.User.GetId());

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int physicianId)
        {
            var physician = this.physicians.Details(physicianId);

            return View(physician);
        }

        public IActionResult Mine()
        {
            var physicianId = this.physicians.GetPhysicianId(this.User.GetId());

            if (physicianId == 0)
            {
                return RedirectToAction(nameof(Become));
            }

            return RedirectToAction("Edit", "Physicians", new { physicianId });
        }
    }
}
