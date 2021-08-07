using MedicReach.Services.Appointments.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Appointments
{
    public interface IAppointmentService
    {
        void Create(string patientId, string physicianId, string date, string hour);

        IEnumerable<AppointmentServiceModel> GetAppointments(string id);
    }
}
