using MedicReach.Infrastructure;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments;
using MedicReach.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IPatientService patients;
        private readonly IAppointmentService appointments;

        public AppointmentsController(IPatientService patients, IAppointmentService appointments)
        {
            this.patients = patients;
            this.appointments = appointments;
        }

        public IActionResult Book(int physicianId)
        {
            var patientId = this.patients.GetPatientId(this.User.GetId());

            return View(new AppointmentFormModel
            {
                physicianId = physicianId,
                patientId = patientId
            });
        }

        [HttpPost]
        public IActionResult Book(AppointmentFormModel appointment)
        {

            this.appointments.Create(
                appointment.patientId,
                appointment.physicianId,
                appointment.Date,
                appointment.Hour);

            return Redirect("/");            
        }
    }
}
