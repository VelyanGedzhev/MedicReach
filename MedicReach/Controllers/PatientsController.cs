﻿using AutoMapper;
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

        [Authorize]
        public IActionResult Edit()
        {
            var patient = this.patients.GetPatient(this.User.GetId());

            var physicianForm = this.mapper.Map<PatientFormModel>(patient);

            return View(physicianForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(PatientFormModel patient)
        {
            var patientId = this.patients.GetPatientId(this.User.GetId());

            this.patients.Edit(
                patientId,
                patient.FullName,
                patient.Gender);

            this.TempData[GlobalMessageKey] = EditPatientSuccessMessage;

            return RedirectToAction("Index", "Home");
        }
    }
}
