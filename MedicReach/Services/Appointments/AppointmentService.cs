using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Services.Appointments.Models;
using MedicReach.Services.Appointments.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MedicReach.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;

        public AppointmentService(MedicReachDbContext data, IMapper mapper)
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

            var dateParse = DateTime.TryParseExact(
                completeDate,
                "dd-MM-yyyy:HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime appointmantDate);

            if (!dateParse || appointmantDate < DateTime.UtcNow)
            {
                return false;
            }

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

        public AllMyAppointmentsQueryServiceModel All(
            string id,
            AppointmentSorting sorting, 
            int currentPage, 
            int appointmentsPerPage)
        {
            var appointmentsQuery = this.data
                .Appointments
                .Where(a => a.PatientId == id || a.PhysicianId == id)
                .AsQueryable();

            appointmentsQuery = sorting switch
            {
                AppointmentSorting.Approved => appointmentsQuery.OrderByDescending(a => a.IsApproved).ThenBy(a => a.Date),
                AppointmentSorting.Unapproved or _ => appointmentsQuery.OrderBy(a => a.IsApproved).ThenBy(a => a.Date),
                
            };

            var totalAppointments = appointmentsQuery.Count();

            var appointments = appointmentsQuery
                .Skip((currentPage - 1) * appointmentsPerPage)
                .Take(appointmentsPerPage)
                .ProjectTo<AppointmentServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return new AllMyAppointmentsQueryServiceModel
            {
                TotalAppointments = totalAppointments,
                CurrentPage = currentPage,
                Appointments = appointments
            };
        }

        public IEnumerable<AppointmentServiceModel> GetAppointments(string id)
            => this.data
                .Appointments
                .Where(p => p.PatientId == id || p.PhysicianId == id)
                .ProjectTo<AppointmentServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(a => a.Date)
                .ToList();

        public AppointmentServiceModel GetAppointment(string id)
            => this.data
                .Appointments
                .Where(p => p.Id == id)
                .ProjectTo<AppointmentServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

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
