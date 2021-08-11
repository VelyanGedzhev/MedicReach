using MedicReach.Services.Reviews.Models.NewFolder;
using System.Collections.Generic;

namespace MedicReach.Services.Reviews.Models
{
    public class AllReviewsQueryModel
    {
        public const int ReviewsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string PhysicianId { get; set; }

        public ReviewsSorting Sorting { get; set; }

        public IEnumerable<ReviewServiceModel> Reviews { get; set; }

        public int TotalReviews { get; set; }
    }
}
