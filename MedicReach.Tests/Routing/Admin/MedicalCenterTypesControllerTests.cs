using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.MedicalCenterTypes;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class MedicalCenterTypesControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/MedicalCenterTypes/Add")
                .To<MedicalCenterTypesController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/MedicalCenterTypes/Add")
                    .WithMethod(HttpMethod.Post))
                .To<MedicalCenterTypesController>(c => c.Add(With.Any<TypeFormModel>()));
    }
}
