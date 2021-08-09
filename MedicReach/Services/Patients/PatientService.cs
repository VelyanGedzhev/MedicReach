using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Patients.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MedicReach.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public PatientService(MedicReachDbContext data, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            this.data = data;
            this.userManager = userManager;
            this.mapper = mapper;
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
                await userManager.AddToRoleAsync(user, WebConstants.PatientRoleName);
            })
                .GetAwaiter()
                .GetResult();

            this.data.Patients.Add(patient);
            this.data.SaveChanges();
        }

        public void Edit(string id, string fullName, string gender)
        {
            var patient = this.data.Patients.Find(id);

            patient.FullName = fullName;
            patient.Gender = gender;

            this.data.SaveChanges();
        }

        public string GetPatientId(string userId)
            => this.data
                .Patients
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

        public PatientServiceModel GetPatient(string userId)
            => this.data
                .Patients
                .Where(p => p.UserId == userId)
                .ProjectTo<PatientServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault(); 
    }
}
