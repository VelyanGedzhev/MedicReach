using MedicReach.Models.MedicalCenters.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Services.MedicalCenters.Models
{
    public class MedicalCenterQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int MedicalCentersPerPage { get; init; }

        public string Type { get; init; }

        [Display(Name = "Find by Type")]
        public IEnumerable<string> Types { get; set; }

        public string Country { get; init; }

        [Display(Name = "Find by Country")]
        public IEnumerable<string> Countries { get; set; }

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by:")]
        public MedicalCentersSorting Sorting { get; init; }

        public IEnumerable<MedicalCenterServiceModel> MedicalCenters { get; set; }

        public int TotalMedicalCenters { get; set; }
    }
}
