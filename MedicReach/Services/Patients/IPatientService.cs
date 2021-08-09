using MedicReach.Services.Patients.Models;

namespace MedicReach.Services.Patients
{
    public interface IPatientService
    {
        void Create(string fullname, string gender, string userId);

        void Edit(string id, string fullName, string gender);

        string GetPatientId(string userId);

        PatientServiceModel GetPatient(string userId);
    }
}
