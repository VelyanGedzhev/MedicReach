using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Appointments;
using MedicReach.Services.Reviews.Models;
using System.Linq;

namespace MedicReach.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;

        public ReviewService(MedicReachDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public void Create(
            string patientId, 
            string physicianId, 
            string appointmentId,
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

            var appointment = this.data
                .Appointments
                .FirstOrDefault(a => a.Id == appointmentId);

            appointment.IsReviewed = true;

            this.data.Reviews.Add(review);
            this.data.SaveChanges();
        }

        public ReviewServiceModel GetLastReview(string physicianId)
            => this.data
                .Reviews
                .Where(r => r.PhysicianId == physicianId)
                .OrderByDescending(r => r.CreatedOn)
                .ProjectTo<ReviewServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

        public double GetAverageReviewRating(string physicianId)
            => this.data
                .Reviews
                .Where(r => r.PhysicianId == physicianId)
                .Average(r => r.Rating);
    }
}
