using MedicReach.Data;
using MedicReach.Data.Models;
using System.Linq;

namespace MedicReach.Areas.Admin.Services.Specialities
{
    public class SpecialityService : ISpecialityService
    {
        private readonly MedicReachDbContext data;

        public SpecialityService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public void Add(string name)
        {
            var speciality = new PhysicianSpeciality
            {
                Name = name
            };

            this.data.PhysicianSpecialities.Add(speciality);
            this.data.SaveChanges();
        }

        public bool IsExisting(string name)
            => this.data
                .PhysicianSpecialities
                .Any(s => s.Name == name);
    }
}
