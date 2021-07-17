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

            SeedAddresses(data);
            SeedSpecialities(data);

            return app;
        }

        private static void SeedAddresses(MedicReachDbContext data)
        {
            if (data.Addresses.Any())
            {
                return;
            }

            data.Addresses.AddRange(new[]
            {
                new Address { Name = "Abbey Road", Number = 3, City = "London"},
                new Address { Name = "Royal Mile", Number = 5, City = "Edinburgh"},
                new Address { Name = "Orchard Road", Number = 2, City = "Singapore"},
                new Address { Name = "Broadway", Number = 13, City = "New York City"}
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
                new PhysicianSpeciality { Name = "Neurologist"},
            });

            data.SaveChanges();
        }
    }
}
