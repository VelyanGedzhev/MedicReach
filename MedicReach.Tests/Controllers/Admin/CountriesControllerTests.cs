using MedicReach.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using MedicReach.Areas.Admin.Models.Countries;
using MedicReach.Data.Models;
using System.Linq;
using MedicReach.Tests.Data;

namespace MedicReach.Tests.Controllers.Admin
{
    public class CountriesControllerTests
    {
        [Fact]
        public void AddActionShouldReturnView()
            => MyController<CountriesController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Add())
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("CountryName", "CNC")]
        public void AddPostActionShouldRedirectCorrectly(string countryName, string countryCode)
            => MyController<CountriesController>
                .Instance()
                .Calling(c => 
                    c.Add(new CountryFormModel
                    {
                        Name = countryName,
                        Alpha3Code = countryCode
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .Data(data => data
                    .WithSet<Country>(country => country
                        .Any(c => 
                            c.Name == countryName &&
                            c.Alpha3Code == countryCode)))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Add");

        [Theory]
        [InlineData("CountryName", "CN")]
        [InlineData("CountryName", "CNC")]
        public void AddPostActionShouldReturnViewIfModelStateInvalid(string countryName, string countryCode)
            => MyController<CountriesController>
                .Instance(instance => instance
                    .WithData(Countries.GetCountries(countryName, countryCode)))
                .Calling(c =>
                    c.Add(new CountryFormModel
                    {
                        Name = countryName,
                        Alpha3Code = countryCode
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CountryFormModel>()
                    .Passing(c =>
                        c.Name == countryName &&
                        c.Alpha3Code == countryCode));
    }
}
