using MedicReach.Infrastructure;
using MedicReach.Models.Patients;
using MedicReach.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService patients;

        public PatientsController(IPatientService patients)
        {
            this.patients = patients;
        }

        public IActionResult Become()
        {
            return View(new PatientFormModel());
        }

        [HttpPost]
        public IActionResult Become(PatientFormModel patient)
        {
            this.patients.Create(patient.Gender, this.User.GetId());

            return Redirect("/");
        }
    }
}
