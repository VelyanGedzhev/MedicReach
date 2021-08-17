using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public class MedicalCenterTypes
    {
        public static IEnumerable<MedicalCenterType> GetTypes(string name = null)
        {
            var types = Enumerable.Range(0, 3).Select(p => new MedicalCenterType
            {
                MedicalCenters = new List<MedicalCenter>()
            })
            .ToList();

            var type = new MedicalCenterType
            {
                Name = name,
                MedicalCenters = new List<MedicalCenter>()
            };

            types.Add(type);

            return types;
        }

        public static MedicalCenterType GetMedicalCenterType()
        {
            return new MedicalCenterType
            {
                MedicalCenters = new List<MedicalCenter>()
            };
        }
    }
}
