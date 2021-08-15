using MedicReach.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public class Patients
    {
        public static IEnumerable<Patient> GetPatients(
            string patientId, 
            string fullName, 
            string gender, 
            string userId = null)
        {
            var patients = Enumerable.Range(0, 3).Select(p => new Patient
            {
            })
            .ToList();

            var patient = new Patient
            {
                Id = patientId,
                FullName = fullName,
                Gender = gender,
                UserId = userId,
            };

            patients.Add(patient);

            return patients;

        }
    }
}
