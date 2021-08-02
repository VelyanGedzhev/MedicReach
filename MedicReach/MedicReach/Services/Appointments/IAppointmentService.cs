namespace MedicReach.Services.Appointments
{
    public interface IAppointmentService
    {
        void Create(int patientId, int physicianId, string date, string hour);
    }
}
