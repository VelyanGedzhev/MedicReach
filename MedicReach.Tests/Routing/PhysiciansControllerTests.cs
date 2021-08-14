using MedicReach.Controllers;
using MedicReach.Models.Physicians;
using MyTested.AspNetCore.Mvc;
using System;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class PhysiciansControllerTests
    {
        [Fact]
        public void BecomeActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Physicians/Become")
                .To<PhysiciansController>(c => c.Become());

        [Fact]
        public void BecomePostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Physicians/Become")
                    .WithMethod(HttpMethod.Post))
                .To<PhysiciansController>(c => c.Become(With.Any<PhysicianFormModel>()));

        [Fact]
        public void EditActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Physicians/Edit")
                .To<PhysiciansController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void EditPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Physicians/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<PhysiciansController>(c => c.Edit(With.Any<string>(), With.Any<PhysicianFormModel>()));

        [Fact]
        public void AllActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("Physicians/All")
                .To<PhysiciansController>(c => c.All(With.Any<AllPhysiciansQueryModel>()));

        [Fact]
        public void DetailsActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Physicians/Details")
                .To<PhysiciansController>(c => c.Details(With.Any<string>()));

        [Fact]
        public void MineActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Physicians/Mine")
                .To<PhysiciansController>(c => c.Mine());
    }
}
