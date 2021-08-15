using MedicReach.Data.Models;
using System.Collections.Generic;

namespace MedicReach.Tests.Data
{
    public static class Specialities
    {
        public static PhysicianSpeciality GetSpeciality(int specialityId)
        {
            return new PhysicianSpeciality
            {
                Id = specialityId,
                Physicians = new List<Physician>()
            };
        }
    }
}
