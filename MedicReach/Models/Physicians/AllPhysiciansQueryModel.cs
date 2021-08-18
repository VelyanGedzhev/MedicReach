using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.Physicians
{
    public class AllPhysiciansQueryModel
    {
        public const int PhysiciansPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public string Speciality { get; init; }

        [Display(Name = "Find by Speciality")]
        public IEnumerable<string> Specialities { get; set; }

        [Display(Name = "Find by Medical Center")]
        public IEnumerable<string> MedicalCenters { get; set; }

        public string MedicalCenter { get; init; }

        public bool Approved { get; set; } = true;

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by")]
        public PhysicianSorting Sorting { get; init; }

        public IEnumerable<PhysicianServiceModel> Physicians { get; set; }

        public int TotalPhysicians { get; set; }
    }
}
