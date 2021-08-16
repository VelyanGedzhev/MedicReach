using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Countries;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class CountriesControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Countries/Add")
                .To<CountriesController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Countries/Add")
                    .WithMethod(HttpMethod.Post))
                .To<CountriesController>(c => c.Add(With.Any<CountryFormModel>()));
    }
}
