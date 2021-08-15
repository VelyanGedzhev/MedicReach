using MedicReach.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicReach.Tests.Data
{
    public static class Physicians
    {
        public static IEnumerable<Physician> GetPhysicians(string physicianId, string userId = null)
        {
                var physicians =  Enumerable.Range(0, 3).Select(p => new Physician
                {
                    MedicalCenter = new MedicalCenter
                    {
                        Country = new Country {  },
                        City = new City { },
                        Type = new MedicalCenterType { }
                    },
                    Speciality = new PhysicianSpeciality { }
                })
                .ToList();

            var physician = new Physician
            {
                Id = physicianId,
                UserId = userId,
                PracticePermissionNumber = "PP1234599",
                MedicalCenter = new MedicalCenter
                {
                    Id = "MedicalCenterId",
                    JoiningCode = "MedicalCenter",
                    Country = new Country {  },
                    City = new City {  },
                    Type = new MedicalCenterType {  }
                },
                Speciality = new PhysicianSpeciality {  }
            };

            physicians.Add(physician);

            return physicians;
            
        }

        public static Physician GetPhysician(string physicianId, string userId = null)
        {
            return new Physician
            {
                Id = physicianId,
                UserId = userId,
                PracticePermissionNumber = "PP1234599",
                MedicalCenter = new MedicalCenter
                {
                    Id = "MedicalCenterId",
                    JoiningCode = "MedicalCenter",
                    Country = new Country {  },
                    City = new City {  },
                    Type = new MedicalCenterType {  }
                },
                Speciality = new PhysicianSpeciality {  }
            };
        }
    }
}
