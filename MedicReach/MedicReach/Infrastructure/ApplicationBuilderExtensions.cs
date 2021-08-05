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
using static MedicReach.GlobalConstants;

namespace MedicReach.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedApplicationRoles(services);
            SeedAdministrator(services);
            SeedCountries(services);
            SeedAddresses(services);
            SeedMedicalCenterTypes(services);
            SeedSpecialities(services);

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

        private static void SeedAddresses(IServiceProvider services)
        {
            var data = services.GetRequiredService<MedicReachDbContext>();

            if (data.Addresses.Any())
            {
                return;
            }

            data.Addresses.AddRange(new[]
            {
                new Address { Name = "Abbey Road", Number = 3, City = "London", CountryId = 5},
                new Address { Name = "Royal Mile", Number = 5, City = "Edinburgh", CountryId = 5},
                new Address { Name = "Oranienburger Strasse", Number = 2, City = "Berlin", CountryId = 7},
                new Address { Name = "Broadway", Number = 13, City = "New York City", CountryId = 6}
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
