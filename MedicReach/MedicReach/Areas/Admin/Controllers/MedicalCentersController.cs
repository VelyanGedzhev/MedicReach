using MedicReach.Services.MedicalCenters;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Areas.Admin.Controllers
{
    public class MedicalCentersController : AdminController
    {
        private readonly IMedicalCenterService medicalCenters;

        public MedicalCentersController(IMedicalCenterService medicalCenters) 
            => this.medicalCenters = medicalCenters;

        public IActionResult All()
        {
            var medicalCenters = this.medicalCenters
                    .All()
                    .MedicalCenters;

            return View(medicalCenters);
        }
    }
}
