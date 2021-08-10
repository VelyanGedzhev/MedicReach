using MedicReach.Services.Appointments.Models;
using MedicReach.Services.Appointments.Models.Enums;
using System.Collections.Generic;

namespace MedicReach.Services.Appointments
{
    public interface IAppointmentService
    {
        bool Create(
            string patientId, 
            string physicianId, 
            string date, 
            string hour);

        AllMyAppointmentsQueryServiceModel All(
                string id,
                AppointmentSorting sorting,
                int CurrentPage,
                int appointmentsPerPage);

        IEnumerable<AppointmentServiceModel> GetAppointments(string id);

        AppointmentServiceModel GetAppointment(string id);

        void ChangeApprovalStatus(string appointmentId);
    }
}
