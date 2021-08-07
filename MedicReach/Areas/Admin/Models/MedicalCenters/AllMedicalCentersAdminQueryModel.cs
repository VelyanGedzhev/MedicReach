using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Areas.Admin.Models.MedicalCenters
{
    public class AllMedicalCentersAdminQueryModel
    {
        public const int MedicalCentersPerPage = 1;

        public int CurrentPage { get; init; } = 1;

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        public IEnumerable<MedicalCenterServiceModel> MedicalCenters { get; set; }

        public int TotalMedicalCenters { get; set; }
    }
}
