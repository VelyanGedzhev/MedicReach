﻿using MedicReach.Infrastructure;
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
                PhysicianId = physicianId,
                PatientId = patientId
            });
        }

        [HttpPost]
        [Authorize(Roles = PatientRoleName)]
        public IActionResult Book(AppointmentFormModel appointment)
        {
            var isCreated = this.appointments.Create(
                appointment.PatientId,
                appointment.PhysicianId,
                appointment.Date,
                appointment.Hour);

            if (!isCreated)
            {
                return View(appointment);
            }

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