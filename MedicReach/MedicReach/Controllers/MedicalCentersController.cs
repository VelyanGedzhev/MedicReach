using AutoMapper;
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
        private readonly IMapper mapper;

        public MedicalCentersController(
            IMedicalCenterService medicalCenters,
            IMapper mapper)
        {
            this.medicalCenters = medicalCenters;
            this.mapper = mapper;
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
        public IActionResult Add(MedicalCenterFormModel medicalCenterModel)
        {
            if (!this.medicalCenters.MedicalCenterAddressExists(medicalCenterModel.AddressId))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.AddressId), "Address does not exist.");
            }

            if (!this.medicalCenters.MedicalCenterTypeExists(medicalCenterModel.TypeId))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.TypeId), "Medical Center Type does not exist.");
            }

            if (this.medicalCenters.IsJoiningCodeUsed(medicalCenterModel.JoiningCode))
            {
                this.ModelState.AddModelError(nameof(medicalCenterModel.JoiningCode), "Joining Code already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                medicalCenterModel.Addresses = this.medicalCenters.GetAddresses();
                medicalCenterModel.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();

                return View(medicalCenterModel);
            }

            this.medicalCenters.Create(
                medicalCenterModel.Name,
                medicalCenterModel.AddressId,
                medicalCenterModel.TypeId,
                medicalCenterModel.Description,
                medicalCenterModel.JoiningCode,
                this.User.GetId(),
                medicalCenterModel.ImageUrl);

            //TODO: better way to create medical center while created physician
            return Redirect("/Physicians/Become");
        }

        [Authorize]
        public IActionResult Edit(int medicalCenterId)
        {
            var isUserCreator = this.medicalCenters.IsCreator(User.GetId(), medicalCenterId);

            if (!isUserCreator && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            var medicalCenterForm = this.mapper.Map<MedicalCenterFormModel>(medicalCenter);
            medicalCenterForm.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();
            medicalCenterForm.Addresses = this.medicalCenters.GetAddresses();

            return View(medicalCenterForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int medicalCenterId, MedicalCenterFormModel medicalCenterModel)
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

            if (!this.ModelState.IsValid)
            {
                medicalCenterModel.Addresses = this.medicalCenters.GetAddresses();
                medicalCenterModel.MedicalCenterTypes = this.medicalCenters.GetMedicalCenterTypes();

                return View(medicalCenterModel);
            }

            this.medicalCenters.Edit(
                medicalCenterId,
                medicalCenterModel.Name,
                medicalCenterModel.AddressId,
                medicalCenterModel.TypeId,
                medicalCenterModel.Description,
                medicalCenterModel.JoiningCode,
                medicalCenterModel.ImageUrl);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int medicalCenterId)
        {
            var medicalCenter = this.medicalCenters.Details(medicalCenterId);

            return View(medicalCenter);
        }

        public IActionResult Mine()
        {
            var medicalCenterId = this.medicalCenters.GetMedicalCenterIdByUser(this.User.GetId());

            if (medicalCenterId == 0)
            {
                return BadRequest();
            }

            return RedirectToAction("Edit", "MedicalCenters", new { medicalCenterId });
        }
    }
}
