using AutoMapper;
using MedicReach.Infrastructure;
using MedicReach.Models.Patients;
using MedicReach.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static MedicReach.WebConstants;

namespace MedicReach.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService patients;
        private readonly IMapper mapper;
        private readonly SignInManager<IdentityUser> signInManager;

        public PatientsController(
            IPatientService patients,
            IMapper mapper,
            SignInManager<IdentityUser> signInManager 
            )
        {
            this.patients = patients;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Become()
        {
            return View(new PatientFormModel());
        }

        [Authorize]
        [HttpPost]
        public IActionResult Become(PatientFormModel patient)
        {
            this.patients.Create(
                patient.FullName, 
                patient.Gender, 
                this.User.GetId());

            Task.Run(async () =>
            {
                await this.signInManager.SignOutAsync();
            })
                .GetAwaiter()
                .GetResult();

            this.TempData[GlobalSuccessMessageKey] = BecomePatientSuccessMessage;

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = PatientRoleName)]
        public IActionResult Edit()
        {
            var patient = this.patients.GetPatient(this.User.GetId());

            var physicianForm = this.mapper.Map<PatientFormModel>(patient);

            return View(physicianForm);
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost]
        public IActionResult Edit(PatientFormModel patient)
        {
            var patientId = this.patients.GetPatientId(this.User.GetId());

            this.patients.Edit(
                patientId,
                patient.FullName,
                patient.Gender);

            this.TempData[GlobalSuccessMessageKey] = EditPatientSuccessMessage;

            return RedirectToAction("Index", "Home");
        }
    }
}
