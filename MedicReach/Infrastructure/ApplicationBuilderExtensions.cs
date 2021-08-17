using MedicReach.Data;
using MedicReach.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static MedicReach.Areas.Admin.AdminConstants;
using static MedicReach.WebConstants;

namespace MedicReach.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        private const string PatientId = "c81e9be3-7234-4370-b96a-f4000b32a3f3";
        private const string PhysicianId = "6689b628-8568-4c3d-ac31-85c1a23a0b7b";
        private const string PhysicianUserId = "6689b628-8568-4c3d-ac31-85c1a23a0b7b";
        private const string MedicalCenterId = "cd835fb8-a8ad-4476-adaa-8d80c3103261";
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedApplicationRoles(services);
            SeedAdministrator(services);
            SeedCountries(services);
            SeedCities(services);
            SeedMedicalCenterTypes(services);
            SeedSpecialities(services);
            SeedPatient(services);
            SeedMedicalCenter(services);
            SeedPhysician(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            data.Database.Migrate();
        }

        private static void SeedApplicationRoles(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }

                if (await roleManager.RoleExistsAsync(PatientRoleName))
                {
                    return;
                }

                if (await roleManager.RoleExistsAsync(PhysicianRoleName))
                {
                    return;
                }

                var adminRole = new IdentityRole { Name = AdministratorRoleName };
                var patientRole = new IdentityRole { Name = PatientRoleName };
                var physicianRole = new IdentityRole { Name = PhysicianRoleName };

                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(patientRole);
                await roleManager.CreateAsync(physicianRole);
            })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            const string adminEmail = "admin@medicReach.com";
            const string adminPassword = "admin123456";

            if (userManager.Users.Any(u => u.UserName == adminEmail))
            {
                return;
            }

            Task.Run(async () =>
            {
                var user = new IdentityUser
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };

                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, AdministratorRoleName);
            })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedPatient(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var data = services.GetRequiredService<MedicReachDbContext>();

            const string patientEmail = "patient@medicReach.com";
            const string patientPassword = "123456";
            const string patientName = "John Smith";
            const string patientGender = "Male";

            if (userManager.Users.Any(u => u.UserName == patientEmail))
            {
                return;
            }

            var user = new IdentityUser();

            Task.Run(async () =>
            {
                user.Email = patientEmail;
                user.UserName = patientEmail;

                await userManager.CreateAsync(user, patientPassword);
                await userManager.AddToRoleAsync(user, PatientRoleName);
            })
                .GetAwaiter()
                .GetResult();

            var patient = new Patient
            {
                Id = PatientId,
                FullName = patientName,
                Gender = patientGender,
                UserId = user.Id            
            };

            data.Patients.Add(patient);
            data.SaveChanges();
        }

        private static void SeedPhysician(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var data = services.GetRequiredService<MedicReachDbContext>();

            const string physicianEmail = "physician@medicReach.com";
            const string physicianPassword = "123456";
            const string physicianName = "Jessica Morales";
            const string physicianGender = "Female";
            const int examinationPrice = 60;
            const int specialityId = 3;
            const string practicePermissionNumber = "JS123456";
            const bool defaultBool = true;

            if (userManager.Users.Any(u => u.UserName == physicianEmail))
            {
                return;
            }

            Task.Run(async () =>
            {
                var user = new IdentityUser
                {
                    Id = PhysicianUserId,
                    Email = physicianEmail,
                    UserName = physicianEmail
                };

                await userManager.CreateAsync(user, physicianPassword);
                await userManager.AddToRoleAsync(user, PhysicianRoleName);
            })
                .GetAwaiter()
                .GetResult();

            var physician = new Physician
            {
                Id = PhysicianId,
                FullName = physicianName,
                Gender = physicianGender,
                UserId = PhysicianUserId,
                ExaminationPrice = examinationPrice,
                MedicalCenterId = MedicalCenterId,
                PracticePermissionNumber = practicePermissionNumber,
                IsWorkingWithChildren = defaultBool,
                IsApproved = defaultBool,
                SpecialityId = specialityId,
                ImageUrl = DataConstants.Physician.DefaultFemaleImageUrl
            };

            data.Physicians.Add(physician);
            data.SaveChanges();
        }

        private static void SeedMedicalCenter(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.MedicalCenters.Any(mc => mc.Id == MedicalCenterId))
            {
                return;
            }

            const string name = "JesMor";
            const string joiningCode = "JesMorClinic";
            const string address = "27 Abbey Road";
            const string description = "Best clinic in Buenos Aires";
            const string city = "Buenos Aires";
            const string country = "Argentina";
            const int typeId = 2;

            int cityId = data.Cities.Where(c => c.Name == city).Select(c => c.Id).FirstOrDefault();
            int countryId = data.Countries.Where(c => c.Name == country).Select(c => c.Id).FirstOrDefault();

            var medicalCenter = new MedicalCenter
            {
                Id = MedicalCenterId,
                Name = name,
                JoiningCode = joiningCode,
                Address = address,
                CityId = cityId,
                CountryId = countryId,
                Description = description,
                TypeId = typeId,
                CreatorId = PhysicianUserId,
                ImageUrl = DataConstants.MedicalCenter.DefaultImageUrl
            };

            data.MedicalCenters.Add(medicalCenter);
            data.SaveChanges();
        }

        private static void SeedCountries(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.Countries.Any())
            {
                return;
            }

            data.Countries.AddRange(new[]
            {
                new Country { Name = "Bulgaria", Alpha3Code = "BGR"},
                new Country { Name = "France", Alpha3Code = "FRA"},
                new Country { Name = "Argentina", Alpha3Code = "ARG"},
                new Country { Name = "Netherlands", Alpha3Code = "NLD"},
                new Country { Name = "United Kingdom of Great Britain and Northern Ireland", Alpha3Code = "GBR"},
                new Country { Name = "United States of America", Alpha3Code = "USA"},
                new Country { Name = "Germany", Alpha3Code = "DEU"}
            });

            data.SaveChanges();
        }

        private static void SeedCities(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.Cities.Any())
            {
                return;
            }

            data.Cities.AddRange(new[]
            {
                new City { Name = "New York", CountryId = 6},
                new City { Name = "Sofia", CountryId = 1},
                new City { Name = "Berlin", CountryId = 7},
                new City { Name = "Paris", CountryId = 2},
                new City { Name = "Buenos Aires", CountryId = 3},
                new City { Name = "London", CountryId = 5},
                new City { Name = "Edinburgh", CountryId = 5},
                new City { Name = "Amsterdam", CountryId = 4}
            });

            data.SaveChanges();
        }

        private static void SeedMedicalCenterTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.MedicalCenterTypes.Any())
            {
                return;
            }

            data.MedicalCenterTypes.AddRange(new[]
            {
                new MedicalCenterType { Name = "Doctor's Office"},
                new MedicalCenterType { Name = "Clinic"},
                new MedicalCenterType { Name = "Hospital"}
            });

            data.SaveChanges();
        }

        private static void SeedSpecialities(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.PhysicianSpecialities.Any())
            {
                return;
            }

            data.PhysicianSpecialities.AddRange(new[]
            {
                new PhysicianSpeciality { Name = "Family Physician"},
                new PhysicianSpeciality { Name = "Pediatrician"},
                new PhysicianSpeciality { Name = "Cardiologist"},
                new PhysicianSpeciality { Name = "Gastroenterologist"},
                new PhysicianSpeciality { Name = "Oncologist"},
                new PhysicianSpeciality { Name = "Pulmonologist"},
                new PhysicianSpeciality { Name = "Infectious Disease"},
                new PhysicianSpeciality { Name = "Neurologist"},
                new PhysicianSpeciality { Name = "Dermatologist"},
            });

            data.SaveChanges();
        }
    }
}
