using MedicReach.Services.Reviews.Models;

namespace MedicReach.Services.Reviews
{
    public interface IReviewService
    {
        void Create(
            string patientId,
            string physicianId,
            int rating,
            string comment);

        ReviewServiceModel GetLastReview(string physicianId);

        double GetAverageReviewRating(string physicianId);
    }
}
