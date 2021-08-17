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
        private const int HomePageCarouselMedicalCentersCount = 3;

        [Fact]
        public void IndexActionShouldReturnViewWithCorrectModel()
            => MyController<HomeController>
                .Instance()
                .WithData(TenApprovedMedicalCenters)
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<MedicalCenterServiceModel>>()
                    .Passing(model => model.Should().HaveCount(HomePageCarouselMedicalCentersCount)));

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
