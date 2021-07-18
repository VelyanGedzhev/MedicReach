using MedicReach.Data;
using MedicReach.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MedicReach.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<MedicReachDbContext>();

            data.Database.Migrate();

            SeedCountries(data);
            SeedAddresses(data);
            SeedMedicalCenterTypes(data);
            SeedSpecialities(data);

            return app;
        }
        private static void SeedCountries(MedicReachDbContext data)
        {
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

        private static void SeedAddresses(MedicReachDbContext data)
        {
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

        private static void SeedMedicalCenterTypes(MedicReachDbContext data)
        {
            if (data.MedicalCenterTypes.Any())
            {
                return;
            }

            data.MedicalCenterTypes.AddRange(new[]
            {
                new MedicalCenterType { Name = "The Doctor's Office"},
                new MedicalCenterType { Name = "Clinic"},
                new MedicalCenterType { Name = "Hospital"}
            });

            data.SaveChanges();
        }

        private static void SeedSpecialities(MedicReachDbContext data)
        {
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
