using MedicReach.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using MedicReach.Areas.Admin.Models.Cities;

namespace MedicReach.Tests.Routing.Admin
{
    public class CitiesControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Cities/Add")
                .To<CitiesController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Cities/Add")
                    .WithMethod(HttpMethod.Post))
                .To<CitiesController>(c => c.Add(With.Any<CityFormModel>()));
    }
}
