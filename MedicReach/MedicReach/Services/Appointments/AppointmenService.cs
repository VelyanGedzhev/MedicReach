using MedicReach.Data;
using MedicReach.Data.Models;
using System;
using System.Globalization;
using System.Linq;

namespace MedicReach.Services.Appointments
{
    public class AppointmenService : IAppointmentService
    {
        private readonly MedicReachDbContext data;

        public AppointmenService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public void Create(int patientId, int physicianId, string date, string hour)
        {
            var completeDate = date + ":" + hour;
            var appointmantDate = DateTime.ParseExact(completeDate, "dd-MM-yyyy:H:mm", CultureInfo.InvariantCulture);

            var isAvailable = this.data
                .Appointments
                .Any(a => a.Date == appointmantDate);

            var appointment = new Appointment
            {
                PatientId = patientId,
                PhysicianId = physicianId,
                Date = appointmantDate,
                IsAvailable = isAvailable
            };

            this.data.Appointments.Add(appointment);
            this.data.SaveChanges();
        }
    }
}
