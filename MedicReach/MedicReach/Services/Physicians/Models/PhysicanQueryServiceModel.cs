using MedicReach.Models.Physicians.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Services.Physicians.Models
{
    public class PhysicanQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int PhysiciansPerPage { get; init; }

        public string Speciality { get; init; }


        [Display(Name = "Find by Speciality")]
        public IEnumerable<string> Specialities { get; set; }


        [Display(Name = "Find by Medical Center")]
        public IEnumerable<string> MedicalCenters { get; set; }

        public string MedicalCenter { get; init; }

        public bool Approved { get; init; }

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by:")]
        public PhysicianSorting Sorting { get; init; }

        public IEnumerable<PhysicianServiceModel> Physicians { get; set; }

        public int TotalPhysicians { get; set; }
    }
}
