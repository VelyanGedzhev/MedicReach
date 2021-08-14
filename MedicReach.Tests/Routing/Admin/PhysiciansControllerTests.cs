using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Physicians;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class PhysiciansControllerTests
    {
        [Fact]
        public void AllActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Physicians/All")
                .To<PhysiciansController>(c => c.All(With.Any<AllPhysiciansAdminQueryModel>()));

        [Fact]
        public void ApproveActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Physicians/Approve")
                .To<PhysiciansController>(c => c.Approve(With.Any<string>()));
    }
}
