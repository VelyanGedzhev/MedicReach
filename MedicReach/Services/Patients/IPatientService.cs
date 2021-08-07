namespace MedicReach.Services.Patients
{
    public interface IPatientService
    {
        void Create(string fullname, string gender, string userId);

        string GetPatientId(string userId);
    }
}
