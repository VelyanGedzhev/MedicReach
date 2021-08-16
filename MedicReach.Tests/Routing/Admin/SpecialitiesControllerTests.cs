using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Specialities;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class SpecialitiesControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Specialities/Add")
                .To<SpecialitiesController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Specialities/Add")
                    .WithMethod(HttpMethod.Post))
                .To<SpecialitiesController>(c => c.Add(With.Any<SpecialityFormModel>()));
    }
}
