using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Reviews.Models;
using MedicReach.Services.Reviews.Models.NewFolder;
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

        public AllReviewsQueryModel AllReviews(
            string physicianId,
            ReviewsSorting sorting,
            int currentPage, 
            int reviewsPerPage)
        {
            var reviewsQuery = this.data
                .Reviews
                .Where(a => a.PhysicianId == physicianId)
                .AsQueryable();

            reviewsQuery = sorting switch
            {
                ReviewsSorting.HighestRating => reviewsQuery.OrderByDescending(r => r.Rating),
                ReviewsSorting.LowestRating => reviewsQuery.OrderBy(r => r.Rating),
                ReviewsSorting.Oldest => reviewsQuery.OrderByDescending(a => a.CreatedOn),
                ReviewsSorting.Newest or _ => reviewsQuery.OrderBy(a => a.CreatedOn)
            };

            var totalReviews = reviewsQuery.Count();

            var reviews = reviewsQuery
                .Skip((currentPage - 1) * reviewsPerPage)
                .Take(reviewsPerPage)
                .ProjectTo<ReviewServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return new AllReviewsQueryModel
            {
                TotalReviews = totalReviews,
                CurrentPage = currentPage,
                Reviews = reviews
            };
        }
    }
}
