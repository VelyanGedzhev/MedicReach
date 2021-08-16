using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.Cities;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static MedicReach.Areas.Admin.AdminConstants;

namespace MedicReach.Tests.Controllers.Admin
{
    public class CitiesControllerTests
    {
        [Fact]
        public void AddActionShouldReturnViewWithCorrectModel()
            => MyController<CitiesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Countries.GetCountries()))
                .Calling(c => c.Add())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CityFormModel>());

        [Theory]
        [InlineData("CityName", 2)]
        public void AddPostActionShouldRedirectCorrectly(string cityName, int countryId)
            => MyController<CitiesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Countries.GetCountries()))
                .Calling(c => c
                    .Add(new CityFormModel
                    {
                        Name = cityName,
                        CountryId = countryId
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .TempData(temp => temp 
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction("Add");

        [Theory]
        [InlineData("CityName", 2)]
        public void AddPostActionShouldReturnViewWhenModelStateInvalid(string cityName, int countryId)
            => MyController<CitiesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Cities.GetCity(cityName, countryId)))
                .Calling(c => c
                    .Add(new CityFormModel
                    {
                        Name = cityName,
                        CountryId = countryId
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CityFormModel>()
                    .Passing(c => 
                        c.Name == cityName &&
                        c.CountryId == countryId));
    }
}
