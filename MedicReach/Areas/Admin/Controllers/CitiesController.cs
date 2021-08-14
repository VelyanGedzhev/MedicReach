using MedicReach.Areas.Admin.Models.Cities;
using MedicReach.Services.Cities;
using MedicReach.Services.Coutries;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Areas.Admin.Controllers
{
    public class CitiesController : AdminController 
    {
        private readonly ICityService cities;
        private readonly ICountryService countries;

        public CitiesController(ICityService cities, ICountryService countries)
        {
            this.cities = cities;
            this.countries = countries;
        }

        public IActionResult Add()
        {
            return View(new CityFormModel
            {
                Countries = this.countries.GetCountries()
            });
        }

        [HttpPost]
        public IActionResult Add(CityFormModel city)
        {
            if (this.cities.IsExisting(city.Name))
            {
                this.ModelState.AddModelError(nameof(city.Name), "City already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                city.Countries = this.countries.GetCountries();
                return View(city);
            }

            this.cities.Add(city.Name, city.CountryId);

            this.TempData[WebConstants.GlobalSuccessMessageKey] = string.Format(WebConstants.AddCitySuccessMessage, city.Name);

            return RedirectToAction(nameof(Add));
        }
    }
}
