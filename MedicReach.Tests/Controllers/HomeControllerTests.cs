using FluentAssertions;
using MedicReach.Controllers;
using MedicReach.Models;
using MedicReach.Services.MedicalCenters.Models;
using MyTested.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using static MedicReach.Tests.Data.MedicalCenters;

namespace MedicReach.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionShouldReturnViewWithCorrectModel()
            => MyController<HomeController>
                .Instance(instance => instance
                    .WithData(GetMedicalCenters))
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<MedicalCenterServiceModel>>()
                    .Passing(model => model.Should().HaveCount(3)));

        [Fact]
        public void ErrorActionShouldReturnViewWithCorrectModel()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
    }
}
