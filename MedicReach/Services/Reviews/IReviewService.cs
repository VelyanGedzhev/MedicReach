using MedicReach.Services.Reviews.Models;
using MedicReach.Services.Reviews.Models.NewFolder;

namespace MedicReach.Services.Reviews
{
    public interface IReviewService
    {
        void Create(
            string patientId,
            string physicianId,
            string appointmentId,
            int rating,
            string comment);

        ReviewServiceModel GetLastReview(string physicianId);

        double GetAverageReviewRating(string physicianId);

        AllReviewsQueryModel AllReviews(
            string physicianId,
            ReviewsSorting sorting,
            int currentPage,
            int reviewsPerPage);
    }
}
