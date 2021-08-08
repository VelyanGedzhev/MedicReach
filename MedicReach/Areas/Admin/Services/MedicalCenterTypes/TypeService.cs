using MedicReach.Data;
using MedicReach.Data.Models;
using System.Linq;

namespace MedicReach.Areas.Admin.Services.MedicalCenterTypes
{
    public class TypeService : ITypeService
    {
        private readonly MedicReachDbContext data;

        public TypeService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public void Add(string name)
        {
            var type = new MedicalCenterType
            {
                Name = name
            };

            this.data.MedicalCenterTypes.Add(type);
            this.data.SaveChanges();
        }

        public bool IsExisting(string name)
            => this.data
                .MedicalCenterTypes
                .Any(s => s.Name == name);
    }
}
