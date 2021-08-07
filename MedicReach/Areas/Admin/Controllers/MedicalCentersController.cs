using MedicReach.Areas.Admin.Models.MedicalCenters;
using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters;
using Microsoft.AspNetCore.Mvc;

namespace MedicReach.Areas.Admin.Controllers
{
    public class MedicalCentersController : AdminController
    {
        private readonly IMedicalCenterService medicalCenters;

        public MedicalCentersController(IMedicalCenterService medicalCenters) 
            => this.medicalCenters = medicalCenters;

        public IActionResult All(AllMedicalCentersAdminQueryModel query)
        {
            var queryResult = this.medicalCenters
                    .All(
                        null,
                        null,
                        query.SearchTerm,
                        MedicalCentersSorting.DateCreated,
                        query.CurrentPage,
                        AllMedicalCentersAdminQueryModel.MedicalCentersPerPage
                        );

            query.MedicalCenters = queryResult.MedicalCenters;
            query.TotalMedicalCenters = queryResult.TotalMedicalCenters;

            return View(query);
        }
    }
}
