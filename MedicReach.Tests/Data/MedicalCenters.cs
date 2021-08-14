using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public static class MedicalCenters
    {
        public static IEnumerable<MedicalCenter> GetMedicalCenters
            => Enumerable.Range(0, 10).Select(mc => new MedicalCenter 
            {
                Physicians = new List<Physician> { new Physician { IsApproved = true} }
            });
    }
}
