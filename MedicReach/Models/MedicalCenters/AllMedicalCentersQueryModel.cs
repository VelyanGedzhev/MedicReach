using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.MedicalCenters
{
    public class AllMedicalCentersQueryModel
    {
        public const int MedicalCentersPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public string Type { get; init; }

        [Display(Name = "Find by Type")]
        public IEnumerable<string> Types { get; set; }

        [Display(Name = "Find by Country")]
        public IEnumerable<string> Countries { get; set; }

        public string Country { get; init; }

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by")]
        public MedicalCentersSorting Sorting { get; init; }

        public IEnumerable<MedicalCenterServiceModel> MedicalCenters { get; set; }

        public int TotalMedicalCenters { get; set; }
    }
}
