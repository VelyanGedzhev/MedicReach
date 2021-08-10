using MedicReach.Infrastructure;
using MedicReach.Models.Reviews;
using MedicReach.Services.Appointments;
using MedicReach.Services.Patients;
using MedicReach.Services.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviews;
        private readonly IAppointmentService appointments;
        private readonly IPatientService patients;

        public ReviewsController(IReviewService reviews, IPatientService patients, IAppointmentService appointments)
        {
            this.reviews = reviews;
            this.patients = patients;
            this.appointments = appointments;
        }

        public IActionResult Write(string appointmentId)
        {
            var appointment = this.appointments.GetAppointment(appointmentId);

            return View(new ReviewFormModel
            {
                PatientId = appointment.PatientId,
                PhysicianId = appointment.PhysicianId,
                AppointmentId = appointment.Id
            });
        }

        [HttpPost]
        public IActionResult Write(ReviewFormModel review)
        {
            this.reviews.Create(
                review.PatientId,
                review.PhysicianId,
                review.AppointmentId,
                review.Rating,
                review.Comment);

            return RedirectToAction("Mine", "Appointments");
        }
    }
}
