using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Reviews.Models;
using System.Linq;

namespace MedicReach.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly MedicReachDbContext data;

        public ReviewService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public void Create(
            string patientId, 
            string physicianId, 
            int rating, 
            string comment)
        {
            var review = new Review
            {
                PatientId = patientId,
                PhysicianId = physicianId,
                Rating = rating,
                Comment = comment
            };

            this.data.Reviews.Add(review);
            this.data.SaveChanges();
        }

        public ReviewServiceModel GetLastReview(string physicianId)
            => this.data
                .Reviews
                .Where(r => r.PhysicianId == physicianId)
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new ReviewServiceModel
                {
                    PatientId = r.PatientId,
                    PhysicianId = r.PhysicianId,
                    Rating = r.Rating,
                    Comment = r.Comment

                })
                .FirstOrDefault();

        public double GetAverageReviewRating(string physicianId)
            => this.data
                .Reviews
                .Where(r => r.PhysicianId == physicianId)
                .Average(r => r.Rating);
    }
}
