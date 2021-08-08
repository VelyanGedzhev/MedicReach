using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MedicReach.Services.Appointments
{
    public class AppointmenService : IAppointmentService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;

        public AppointmenService(MedicReachDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public bool Create(
            string patientId, 
            string physicianId, 
            string date, 
            string hour)
        {
            var completeDate = date + ":" + hour;
            var appointmantDate = DateTime.ParseExact(completeDate, "dd-MM-yyyy:HH:mm", CultureInfo.InvariantCulture);

            bool isbBooked = IsBooked(appointmantDate, physicianId);

            if (isbBooked)
            {
                return false;
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                PhysicianId = physicianId,
                Date = appointmantDate
            };

            this.data.Appointments.Add(appointment);
            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<AppointmentServiceModel> GetAppointments(string id)
            => this.data
                .Appointments
                .Where(p => p.PatientId == id || p.PhysicianId == id)
                .ProjectTo<AppointmentServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(a => a.Date)
                .ToList();

        public void ChangeApprovalStatus(string appointmentId)
        {
            var appointment = this.data.Appointments.Find(appointmentId);

            appointment.IsApproved = !appointment.IsApproved;

            this.data.SaveChanges();
        }

        private bool IsBooked(DateTime appointmantDate, string physicianId) 
            => this.data
                .Appointments
                .Any(a => a.Date == appointmantDate && a.PhysicianId == physicianId);
    }
}
