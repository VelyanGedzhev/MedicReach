using MedicReach.Areas.Admin.Models.Countries;
using MedicReach.Services.Coutries;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Areas.Admin.Controllers
{
    public class CountriesController : AdminController 
    {
        private readonly ICountryService countries;

        public CountriesController(ICountryService countries) 
            => this.countries = countries;

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CountryFormModel country)
        {
            if (this.countries.IsExisting(country.Name))
            {
                this.ModelState.AddModelError(country.Name, "Country already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(country);
            }

            this.countries.Add(country.Name, country.Alpha3Code);

            this.TempData[WebConstants.GlobalSuccessMessageKey] = string.Format(WebConstants.AddCountrySuccessMessage, country.Name, country.Alpha3Code);

            return RedirectToAction(nameof(Add));
        }
    }
}
