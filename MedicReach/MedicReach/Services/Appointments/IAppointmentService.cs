using MedicReach.Services.Appointments.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Appointments
{
    public interface IAppointmentService
    {
        void Create(int patientId, int physicianId, string date, string hour);

        IEnumerable<AppointmentServiceModel> GetPatientAppointments(int patientId);
    }
}
