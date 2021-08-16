using MedicReach.Data.Models;
using System;
using System.Globalization;

namespace MedicReach.Tests.Data
{
    public class Appointments
    {
        public static Appointment GetAppointment(
            string physicianId,
            string patientId,
            string date = null,
            string hour = null,
            string appointmentId = null)
        {
            var appointment = new Appointment
            {
                Id = appointmentId ?? Guid.NewGuid().ToString(),
                PhysicianId = physicianId,
                Physician = new Physician { Id = physicianId },
                PatientId = patientId,
                Patient = new Patient { Id = patientId },
                Date = DateTime.ParseExact($"{date}:{hour}", "dd-MM-yyyy:HH:mm", CultureInfo.InvariantCulture),
            };

            return appointment;
        }
    }
}
