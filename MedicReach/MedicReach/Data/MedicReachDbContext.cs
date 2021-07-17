using MedicReach.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicReach.Data
{
    public class MedicReachDbContext : IdentityDbContext
    {
        public MedicReachDbContext(DbContextOptions<MedicReachDbContext> options)
            : base(options)
        {
        }

        public DbSet<MedicalCenter> MedicalCenters { get; init; }

        public DbSet<Address> Addresses { get; init; }

        public DbSet<Physician> Physicians { get; init; }

        public DbSet<PhysicianSpeciality> PhysicianSpecialities { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<MedicalCenter>()
                .HasOne(a => a.Address)
                .WithMany(a => a.MedicalCenters)
                .HasForeignKey(a => a.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Physician>()
                .HasOne(p => p.Speciality)
                .WithMany(p => p.Physicians)
                .HasForeignKey(p => p.SpecialityId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
