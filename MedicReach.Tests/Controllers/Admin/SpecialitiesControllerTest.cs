using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Specialities;
using MedicReach.Data.Models;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace MedicReach.Tests.Controllers.Admin
{
    public class SpecialitiesControllerTest
    {
        [Fact]
        public void AddActionShouldReturnView()
               => MyController<SpecialitiesController>
                   .Instance(instance => instance
                       .WithUser())
                   .Calling(c => c.Add())
                   .ShouldReturn()
                   .View();

        [Theory]
        [InlineData("SpecialityName")]
        public void AddPostActionShouldRedirectCorrectly(string specialityName)
            => MyController<SpecialitiesController>
                .Instance()
                .Calling(c =>
                    c.Add(new SpecialityFormModel
                    {
                        Name = specialityName,
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .Data(data => data
                    .WithSet<PhysicianSpeciality>(country => country
                        .Any(c =>
                            c.Name == specialityName)))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Add");

        [Theory]
        [InlineData(1, "SpecialityName")]
        public void AddPostActionShouldReturnViewIfModelStateInvalid(int id, string specialityName)
            => MyController<SpecialitiesController>
                .Instance(instance => instance
                    .WithData(Specialities.GetSpeciality(id, specialityName)))
                .Calling(c =>
                    c.Add(new SpecialityFormModel
                    {
                        Name = specialityName
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<SpecialityFormModel>()
                    .Passing(c =>
                        c.Name == specialityName));
    }
}
