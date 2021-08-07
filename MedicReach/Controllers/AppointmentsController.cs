using MedicReach.Infrastructure;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments;
using MedicReach.Services.Patients;
using MedicReach.Services.Physicians;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MedicReach.WebConstants;

namespace MedicReach.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService appointments;
        private readonly IPatientService patients;
        private readonly IPhysicianService physicians;

        public AppointmentsController(
            IAppointmentService appointments,
            IPatientService patients,
            IPhysicianService physicians)
        {
            this.appointments = appointments;
            this.patients = patients;
            this.physicians = physicians;
        }

        [Authorize(Roles = PatientRoleName)]
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
        [Authorize(Roles = PatientRoleName)]
        public IActionResult Book(AppointmentFormModel appointment)
        {
            this.appointments.Create(
                appointment.patientId,
                appointment.physicianId,
                appointment.Date,
                appointment.Hour);

            this.TempData[GlobalMessageKey] = BookAppointmentSuccessMessage; 

            return RedirectToAction(nameof(Mine));            
        }

        [Authorize(Roles = PatientRoleName + "," + PhysicianRoleName)]
        public IActionResult Mine()
        {
            var id = GetId();

            var appointments = this.appointments.GetAppointments(id);

            return View(appointments);
        }

        private string GetId()
        {
            var userId = this.User.GetId();

            if (this.User.IsPhysician())
            {
                return this.physicians.GetPhysicianId(userId);
            }

            return this.patients.GetPatientId(userId);
        }
    }
}
