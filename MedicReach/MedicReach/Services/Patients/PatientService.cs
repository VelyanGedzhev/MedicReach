using MedicReach.Data;
using MedicReach.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MedicReach.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly MedicReachDbContext data;

        private readonly UserManager<IdentityUser> userManager;

        public PatientService(MedicReachDbContext data, UserManager<IdentityUser> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public void Create(string fullname, string gender, string userId)
        {
            var patient = new Patient
            {
                FullName = fullname,
                Gender = gender,
                UserId = userId
            };

            var user = this.data.Users.FirstOrDefault(u => u.Id == userId);

            Task.Run(async () =>
            {
                await userManager.AddToRoleAsync(user, GlobalConstants.PatientRoleName);
            })
                .GetAwaiter()
                .GetResult();

            this.data.Patients.Add(patient);
            this.data.SaveChanges();
        }

        public string GetPatientId(string userId)
            => this.data
                .Patients
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();
    }
}
