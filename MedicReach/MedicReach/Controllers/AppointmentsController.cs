using MedicReach.Infrastructure;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments;
using MedicReach.Services.Patients;
using MedicReach.Services.Physicians;
using MedicReach.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IPatientService patients;
        private readonly IAppointmentService appointments;
        private readonly IUserService users;
        private readonly IPhysicianService physicians;

        public AppointmentsController(
            IPatientService patients, 
            IAppointmentService appointments, 
            IUserService users, 
            IPhysicianService physicians)
        {
            this.patients = patients;
            this.appointments = appointments;
            this.users = users;
            this.physicians = physicians;
        }

        public IActionResult Book(int physicianId)
        {
            int patientId = GetId();

            if (patientId == 0)
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
            var id = GetId();

            var appointments = this.appointments.GetPatientAppointments(id);

            return View(appointments);
        }

        private int GetId()
        {
            var userId = this.User.GetId();

            if (this.users.IsPhysician(userId))
            {
                return this.physicians.GetPhysicianId(userId);
            }

            return this.patients.GetPatientId(userId);
        }
    }
}
