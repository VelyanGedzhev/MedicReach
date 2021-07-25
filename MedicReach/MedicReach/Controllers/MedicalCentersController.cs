using MedicReach.Infrastructure;
using MedicReach.Models.MedicalCenters;
using MedicReach.Services.MedicalCenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Controllers
{
    public class MedicalCentersController : Controller
    {
        private readonly IMedicalCenterService medicalCenters;

        public MedicalCentersController(IMedicalCenterService medicalCenters)
        {
            this.medicalCenters = medicalCenters;
        }

        public IActionResult All([FromQuery]AllMedicalCentersQueryModel query)
        {
            var queryResult = this.medicalCenters.All(
                query.Type,
                query.Country,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllMedicalCentersQueryModel.MedicalCentersPerPage);

            var medicalCentersTypes = this.medicalCenters.AllTypes();
            var countries = this.medicalCenters.AllCountries();

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
                Addresses = this.medicalCenters.GetAddresses()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(MedicalCenterFormModel medicalCenter)
        {
            if (!this.medicalCenters.MedicalCenterAddressExists(medicalCenter.AddressId))
            {
                this.ModelState.AddModelError(nameof(medicalCenter.AddressId), "Address does not exist.");
            }

            if (!this.medicalCenters.MedicalCenterTypeExists(medicalCenter.TypeId))
            {
                this.ModelState.AddModelError(nameof(medicalCenter.TypeId), "Medical Center Type does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenter.Addresses = this.medicalCenters.GetAddresses();
                medicalCenter.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();

                return View(medicalCenter);
            }

            this.medicalCenters.Create(
                medicalCenter.Name,
                medicalCenter.AddressId,
                medicalCenter.TypeId,
                medicalCenter.Description,
                medicalCenter.ImageUrl);

            //TODO: better way to create medical center while created physician
            return RedirectToAction("Become", "Physicians");
        }

        [Authorize]
        public IActionResult Edit(int medicalCenterId)
        {
            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            return View(new MedicalCenterFormModel
            {
                Name = medicalCenter.Name,
                Description = medicalCenter.Description,
                ImageUrl = medicalCenter.ImageUrl,
                AddressId = medicalCenter.AddressId,
                TypeId = medicalCenter.TypeId,
                MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes(),
                Addresses = this.medicalCenters.GetAddresses()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int medicalCenterId, MedicalCenterFormModel medicalCenter)
        {
            this.medicalCenters.Edit(
                medicalCenterId,
                medicalCenter.Name,
                medicalCenter.AddressId,
                medicalCenter.TypeId,
                medicalCenter.Description,
                medicalCenter.ImageUrl);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int medicalCenterId)
        {
            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            return View(medicalCenter);
        }
    }
}
