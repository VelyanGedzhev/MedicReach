using MedicReach.Controllers;
using MedicReach.Models.Reviews;
using MedicReach.Services.Reviews.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class ReviewsControllerTests
    {
        [Fact]
        public void WriteActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Write")
                .To<ReviewsController>(c => c.Write(With.Any<string>()));

        [Fact]
        public void WritePostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Reviews/Write")
                    .WithMethod(HttpMethod.Post))
                .To<ReviewsController>(c => c.Write(With.Any<ReviewFormModel>()));

        [Fact]
        public void AllActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/All")
                .To<ReviewsController>(c => c.All(With.Any<string>(), With.Any<AllReviewsQueryModel>()));
    }
}
