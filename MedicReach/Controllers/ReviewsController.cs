using MedicReach.Models.Reviews;
using MedicReach.Services.Appointments;
using MedicReach.Services.Physicians;
using MedicReach.Services.Reviews;
using MedicReach.Services.Reviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MedicReach.WebConstants;

namespace MedicReach.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviews;
        private readonly IAppointmentService appointments;
        private readonly IPhysicianService physicians;

        public ReviewsController(
            IReviewService reviews, 
            IAppointmentService appointments, 
            IPhysicianService physicians)
        {
            this.reviews = reviews;
            this.appointments = appointments;
            this.physicians = physicians;
        }

        [Authorize(Roles = PatientRoleName)]
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

        [Authorize(Roles = PatientRoleName)]
        [HttpPost]
        public IActionResult Write(ReviewFormModel review)
        {
            this.reviews.Create(
                review.PatientId,
                review.PhysicianId,
                review.AppointmentId,
                review.Rating,
                review.Comment);

            this.TempData[GlobalSuccessMessageKey] = WriteReviewSuccessMessage;

            return RedirectToAction("Mine", "Appointments");
        }

        public IActionResult All(string physicianId, AllReviewsQueryModel query)
        {
            var queryResult = this.reviews.AllReviews(
                physicianId,
                query.Sorting,
                query.CurrentPage,
                AllReviewsQueryModel.ReviewsPerPage);

            query.TotalReviews = queryResult.TotalReviews;
            query.Reviews = queryResult.Reviews;

            return View(query);
        }
    }
}
