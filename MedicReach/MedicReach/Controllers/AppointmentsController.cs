using MedicReach.Infrastructure;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments;
using MedicReach.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IPatientService patients;
        private readonly IAppointmentService appointments;

        public AppointmentsController(
            IPatientService patients, 
            IAppointmentService appointments)
        {
            this.patients = patients;
            this.appointments = appointments;
        }

        public IActionResult Book(string physicianId)
        {
            var userId = this.User.GetId();

            var patientId = this.patients.GetPatientId(userId);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

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

        public IActionResult Mine()
        {
            var userId = this.User.GetId();

            var patientId = this.patients.GetPatientId(userId);

            var appointments = this.appointments.GetPatientAppointments(patientId);

            return View(appointments);
        }


    }
}
