using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;

namespace MedicReach.Areas.Admin.Models.Physicians
{
    public class AllPhysiciansAdminQueryModel
    {
        public const int PhysiciansPerPage = 2;

        public int CurrentPage { get; init; } = 1;

        public bool Approved { get; set; }

        public PhysicianSorting Sorting { get; init; }

        public IEnumerable<PhysicianServiceModel> Physicians { get; set; }

        public int TotalPhysicians { get; set; }
    }
}
