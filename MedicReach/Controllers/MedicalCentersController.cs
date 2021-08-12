using AutoMapper;
using MedicReach.Infrastructure;
using MedicReach.Models.MedicalCenters;
using MedicReach.Services.Cities;
using MedicReach.Services.Coutries;
using MedicReach.Services.MedicalCenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MedicReach.WebConstants;
using static MedicReach.Areas.Admin.AdminConstants;

namespace MedicReach.Controllers
{
    public class MedicalCentersController : Controller
    {
        private readonly IMedicalCenterService medicalCenters;
        private readonly ICityService cities;
        private readonly ICountryService countries;
        private readonly IMapper mapper;

        public MedicalCentersController(
            IMedicalCenterService medicalCenters,
            IMapper mapper,
            ICityService cities, 
            ICountryService countries)
        {
            this.medicalCenters = medicalCenters;
            this.mapper = mapper;
            this.cities = cities;
            this.countries = countries;
        }

        public IActionResult All([FromQuery] AllMedicalCentersQueryModel query)
        {
            var queryResult = this.medicalCenters.All(
                query.Type,
                query.Country,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMedicalCentersQueryModel.MedicalCentersPerPage);

            var medicalCentersTypes = this.medicalCenters.AllTypes();
            var countries = this.countries.AllCountries();

            query.Countries = countries;
            query.Types = medicalCentersTypes;
            query.TotalMedicalCenters = queryResult.TotalMedicalCenters;
            query.MedicalCenters = queryResult.MedicalCenters;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View(new MedicalCenterFormModel
            {
                MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes(),
                Cities = this.cities.GetCities(),
                Coutries = this.countries.GetCountries()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(MedicalCenterFormModel medicalCenterModel)
        {
            if (!this.medicalCenters.MedicalCenterTypeExists(medicalCenterModel.TypeId))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.TypeId), "Medical Center Type does not exist.");
            }

            if (this.medicalCenters.IsJoiningCodeUsed(medicalCenterModel.JoiningCode))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.JoiningCode), "Joining Code already exists.");
            }

            if (!this.cities.IsCityInCountry(medicalCenterModel.CountryId, medicalCenterModel.CityId))
            {
                this.TempData[GlobalErrorMessageKey] = CreateMedicalCenterCityAndCountryDontMatchMessage;

                this.ModelState.AddModelError(nameof(medicalCenterModel.CityId), "City does not match the Country.");
                this.ModelState.AddModelError(nameof(medicalCenterModel.CountryId), "City does not match the Country.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenterModel.Cities = this.cities.GetCities();
                medicalCenterModel.Coutries = this.countries.GetCountries();
                medicalCenterModel.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();

                return View(medicalCenterModel);
            }

            this.medicalCenters.Create(
                medicalCenterModel.Name,
                medicalCenterModel.Address,
                medicalCenterModel.TypeId,
                medicalCenterModel.CityId,
                medicalCenterModel.CountryId,
                medicalCenterModel.Description,
                medicalCenterModel.JoiningCode,
                this.User.GetId(),
                medicalCenterModel.ImageUrl);

            this.TempData[GlobalSuccessMessageKey] = string.Format(CreateMedicalCenterSuccessMessage, medicalCenterModel.Name);

            return RedirectToAction("Become", "Physicians");
        }

        [Authorize(Roles = PhysicianRoleName + "," + AdministratorRoleName)]
        public IActionResult Edit(string medicalCenterId)
        {
            var isUserCreator = this.medicalCenters.IsCreator(User.GetId(), medicalCenterId);

            if (!isUserCreator && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            var medicalCenterForm = this.mapper.Map<MedicalCenterFormModel>(medicalCenter);
            medicalCenterForm.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();
            medicalCenterForm.Cities = this.cities.GetCities();
            medicalCenterForm.Coutries = this.countries.GetCountries();

            return View(medicalCenterForm);
        }

        [Authorize(Roles = PhysicianRoleName + "," + AdministratorRoleName)]
        [HttpPost]
        public IActionResult Edit(string medicalCenterId, MedicalCenterFormModel medicalCenterModel)
        {
            var isUserCreator = this.medicalCenters.IsCreator(User.GetId(), medicalCenterId);

            if (!isUserCreator && !User.IsAdmin())
            {
                return Unauthorized();
            }

            if (medicalCenterModel.JoiningCode != this.medicalCenters.GetJoiningCode(medicalCenterId))
            {
                if (this.medicalCenters.IsJoiningCodeUsed(medicalCenterModel.JoiningCode))
                {
                    this.ModelState.AddModelError(nameof(medicalCenterModel.JoiningCode), "Joining Code already exists.");
                }
            }

            if (!this.cities.IsCityInCountry(medicalCenterModel.CountryId, medicalCenterModel.CityId))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.CityId), "City does not match the Country.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenterModel.Cities = this.cities.GetCities();
                medicalCenterModel.Coutries = this.countries.GetCountries();
                medicalCenterModel.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();

                return View(medicalCenterModel);
            }

            this.medicalCenters.Edit(
                medicalCenterId,
                medicalCenterModel.Name,
                medicalCenterModel.Address,
                medicalCenterModel.TypeId,
                medicalCenterModel.CityId,
                medicalCenterModel.CountryId,
                medicalCenterModel.Description,
                medicalCenterModel.JoiningCode,
                medicalCenterModel.ImageUrl);

            this.TempData[GlobalSuccessMessageKey] = string.Format(EditMedicalCenterSuccessMessage, medicalCenterModel.Name);

            return RedirectToAction(nameof(Details), new { medicalCenterId });
        }

        public IActionResult Details(string medicalCenterId)
        {
            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            return View(medicalCenter);
        }

        [Authorize(Roles = PhysicianRoleName)]
        public IActionResult Mine()
        {
            var medicalCenterId = this.medicalCenters.GetMedicalCenterIdByUser(this.User.GetId());

            if (string.IsNullOrEmpty(medicalCenterId))
            {
                this.TempData[GlobalErrorMessageKey] = EditMedicalCenterErrorMessage;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Edit", "MedicalCenters", new { medicalCenterId });
        }
    }
}
