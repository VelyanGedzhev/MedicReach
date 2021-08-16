using MedicReach.Infrastructure;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments;
using MedicReach.Services.Appointments.Models;
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

            if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(physicianId))
            {
                return BadRequest();
            }

            return View(new AppointmentFormModel
            {
                PhysicianId = physicianId,
                PatientId = patientId
            });
        }

        [HttpPost]
        [Authorize(Roles = PatientRoleName)]
        public IActionResult Book(AppointmentFormModel appointment)
        {
            if (string.IsNullOrEmpty(appointment.Date))
            {
                this.ModelState.AddModelError(nameof(appointment.Date), string.Empty);
            }

            if (string.IsNullOrEmpty(appointment.Hour))
            {
                this.ModelState.AddModelError(nameof(appointment.Hour), string.Empty);
            }

            if (!this.ModelState.IsValid)
            {
                return View(appointment);
            }

            var isCreated = this.appointments.Create(
                appointment.PatientId,
                appointment.PhysicianId,
                appointment.Date,
                appointment.Hour);

            if (!isCreated)
            {
                this.TempData[GlobalErrorMessageKey] = string.Format(AppointmentNotAvailableMessage, appointment.Date, appointment.Hour); 
                return View(appointment);
            }

            this.TempData[GlobalSuccessMessageKey] = BookAppointmentSuccessMessage; 

            return RedirectToAction(nameof(Mine));            
        }

        [Authorize(Roles = PatientRoleName + "," + PhysicianRoleName)]
        public IActionResult Mine([FromQuery] AllMyAppointmentsQueryServiceModel query)
        {
            var id = GetId();

            var queryResult = this.appointments.All(
                id,
                query.Sorting,
                query.CurrentPage,
                AllMyAppointmentsQueryServiceModel.AppointmentsPerPage);

            query.TotalAppointments = queryResult.TotalAppointments;
            query.Appointments = queryResult.Appointments;

            return View(query);
        }

        [Authorize(Roles = PhysicianRoleName)]
        public IActionResult ChangeStatus(string appointmentId)
        {
            this.appointments.ChangeApprovalStatus(appointmentId);

            return RedirectToAction(nameof(Mine));
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
