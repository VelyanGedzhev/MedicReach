using MedicReach.Controllers;
using MedicReach.Models.MedicalCenters;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class MedicalCentersControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/MedicalCenters/Add")
                .To<MedicalCentersController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/MedicalCenters/Add")
                    .WithMethod(HttpMethod.Post))
                .To<MedicalCentersController>(c => c.Add(With.Any<MedicalCenterFormModel>()));

        [Fact]
        public void EditActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/MedicalCenters/Edit")
                .To<MedicalCentersController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void EditPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/MedicalCenters/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<MedicalCentersController>(c => c.Edit(With.Any<string>(), With.Any<MedicalCenterFormModel>()));

        [Fact]
        public void DetailsActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/MedicalCenters/Details")
                .To<MedicalCentersController>(c => c.Details(With.Any<string>()));

        [Fact]
        public void MineActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/MedicalCenters/Mine")
                .To<MedicalCentersController>(c => c.Mine());

        [Fact]
        public void AllActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/MedicalCenters/All")
                .To<MedicalCentersController>(c => c.All(With.Any<AllMedicalCentersQueryModel>()));
    }
}
