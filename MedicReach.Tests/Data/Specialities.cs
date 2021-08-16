using MedicReach.Data.Models;
using System.Collections.Generic;

namespace MedicReach.Tests.Data
{
    public static class Specialities
    {
        public static PhysicianSpeciality GetSpeciality(int specialityId = 0, string name = null)
        {
            return new PhysicianSpeciality
            {
                Id = specialityId,
                Name = name,
                Physicians = new List<Physician>()
            };
        }
    }
}
