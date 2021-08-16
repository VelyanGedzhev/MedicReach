using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.MedicalCenterTypes;
using MedicReach.Data.Models;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace MedicReach.Tests.Controllers.Admin
{
    public class MedicalCenterTypesControllerTests
    {
        [Fact]
        public void AddActionShouldReturnView()
            => MyController<MedicalCenterTypesController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Add())
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("TypeName")]
        public void AddPostActionShouldRedirectCorrectly(string typeName)
            => MyController<MedicalCenterTypesController>
                .Instance()
                .Calling(c =>
                    c.Add(new TypeFormModel
                    {
                        Name = typeName,
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .Data(data => data
                    .WithSet<MedicalCenterType>(type => type
                        .Any(c =>
                            c.Name == typeName)))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Add");

        [Theory]
        [InlineData("TypeName")]
        public void AddPostActionShouldReturnViewIfModelStateInvalid(string typeName)
            => MyController<MedicalCenterTypesController>
                .Instance(instance => instance
                    .WithData(MedicalCenterTypes.GetTypes(typeName)))
                .Calling(c =>
                    c.Add(new TypeFormModel
                    {
                        Name = typeName,
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TypeFormModel>()
                    .Passing(c =>c.Name == typeName));
    }
}
