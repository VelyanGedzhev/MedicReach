using MedicReach.Infrastructure;
using MedicReach.Models.Patients;
using MedicReach.Services.Patients;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static MedicReach.WebConstants;

namespace MedicReach.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService patients;
        private readonly SignInManager<IdentityUser> signInManager;

        public PatientsController(
            IPatientService patients, 
            SignInManager<IdentityUser> signInManager)
        {
            this.patients = patients;
            this.signInManager = signInManager;
        }

        public IActionResult Become()
        {
            return View(new PatientFormModel());
        }

        [HttpPost]
        public IActionResult Become(PatientFormModel patient)
        {
            this.patients.Create(patient.FullName, patient.Gender, this.User.GetId());

            Task.Run(async () =>
            {
                await this.signInManager.SignOutAsync();
            })
                .GetAwaiter()
                .GetResult();

            this.TempData[GlobalMessageKey] = BecomePatientSuccessMessage;

            return RedirectToAction("Index", "Home");
        }
    }
}
