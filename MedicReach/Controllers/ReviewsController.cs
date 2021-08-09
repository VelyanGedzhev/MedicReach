using MedicReach.Infrastructure;
using MedicReach.Models.Reviews;
using MedicReach.Services.Patients;
using MedicReach.Services.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviews;
        private readonly IPatientService patients;

        public ReviewsController(IReviewService reviews, IPatientService patients)
        {
            this.reviews = reviews;
            this.patients = patients;
        }

        public IActionResult Write(string physicianId)
        {
            var patientId = this.patients.GetPatientId(this.User.GetId());

            return View(new ReviewFormModel
            {
                PatientId = patientId,
                PhysicianId = physicianId
            });
        }

        [HttpPost]
        public IActionResult Write(ReviewFormModel review)
        {
            this.reviews.Create(
                review.PatientId,
                review.PhysicianId,
                review.Rating,
                review.Comment);

            return RedirectToAction("Mine", "Appointments");
        }
    }
}
