using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Physicians
{
    public interface IPhysicianService
    {
        PhysicanQueryServiceModel All(
            string Speciality,
            string MedicalCenter,
            string searchTerm,
            PhysicianSorting sorting,
            int currentPage,
            int physiciansPerPage);

        IEnumerable<string> AllMedicalCenters();

        IEnumerable<string> AllSpecialities();
    }
}
