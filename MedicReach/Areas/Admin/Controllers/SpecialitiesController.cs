using MedicReach.Areas.Admin.Models.Specialities;
using MedicReach.Areas.Admin.Services.Specialities;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Areas.Admin.Controllers
{
    public class SpecialitiesController : AdminController
    {
        private readonly ISpecialityService specialities;

        public SpecialitiesController(ISpecialityService specialities) 
            => this.specialities = specialities;

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(SpecialityFormModel speciality)
        {
            if (this.specialities.IsExisting(speciality.Name))
            {
                this.ModelState.AddModelError(nameof(speciality.Name), "Speciality already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                return View(speciality);
            }

            this.specialities.Add(speciality.Name);

            this.TempData[WebConstants.GlobalSuccessMessageKey] = string.Format(WebConstants.AddSpecialitySuccessMessage, speciality.Name);

            return RedirectToAction(nameof(Add));
        }
    }
}
