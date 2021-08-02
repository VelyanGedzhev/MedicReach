namespace MedicReach.Services.Patients
{
    public interface IPatientService
    {
        void Create(string gender, string userId);

        int GetPatientId(string userId);
    }
}
