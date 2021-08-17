using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Physicians;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Controllers.Admin
{
    public class PhysiciansControllerTests
    {
        private const int PhysiciansCount = 4;

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AllActionShouldReturnViewWithAllCurrentlyApprovedPhysicians(bool isApproved)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithData(Physicians.GetPhysicians(TestUser.Identifier, null, isApproved)))
                .Calling(c =>
                    c.All(new AllPhysiciansAdminQueryModel
                    {
                    }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllPhysiciansAdminQueryModel>()
                    .Passing(c => c.TotalPhysicians == PhysiciansCount));

        [Fact]
        public void ApproveActionShouldRedirectCorrectly()
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithData(Physicians.GetPhysician(TestUser.Identifier)))
                .Calling(c => c.Approve(TestUser.Identifier))
                .ShouldReturn()
                .RedirectToAction("All");
    }
}
