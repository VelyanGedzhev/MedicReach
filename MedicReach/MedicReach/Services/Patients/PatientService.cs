using MedicReach.Data;
using MedicReach.Data.Models;
using System.Linq;

namespace MedicReach.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly MedicReachDbContext data;

        public PatientService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public void Create(string gender, string userId)
        {
            var patient = new Patient
            {
                Gender = gender,
                UserId = userId
            };

            this.data.Patients.Add(patient);
            this.data.SaveChanges();
        }

        public int GetPatientId(string userId)
            => this.data
            .Patients
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstOrDefault();
    }
}
