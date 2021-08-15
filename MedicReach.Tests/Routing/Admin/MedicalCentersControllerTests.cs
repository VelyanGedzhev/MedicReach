using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.MedicalCenters;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class MedicalCentersControllerTests
    {
        [Fact]
        public void AllActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/MedicalCenters/All")
                .To<MedicalCentersController>(c => c.All(With.Any<AllMedicalCentersAdminQueryModel>()));
    }
}
