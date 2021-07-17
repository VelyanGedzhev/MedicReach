using MedicReach.Models.Physicians.Enums;
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

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; init; }

        [Display(Name = "Sort by:")]
        public PhysicianSorting Sorting { get; init; }

        public IEnumerable<PhysicianListViewModel> Physicians { get; set; }

        public int TotalPhysiciansCount { get; set; }
    }
}
