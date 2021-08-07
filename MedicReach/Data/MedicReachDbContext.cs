using MedicReach.Data.Models;
using Microsoft.AspNetCore.Identity;
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

        public DbSet<MedicalCenterType> MedicalCenterTypes { get; init; }

        public DbSet<Address> Addresses { get; init; }

        public DbSet<Country> Countries { get; init; }

        public DbSet<Physician> Physicians { get; init; }

        public DbSet<Patient> Patients { get; init; }

        public DbSet<PhysicianSpeciality> PhysicianSpecialities { get; init; }

        public DbSet<Appointment> Appointments { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<MedicalCenter>()
                .HasOne(mc => mc.Address)
                .WithMany(a => a.MedicalCenters)
                .HasForeignKey(mc => mc.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<MedicalCenter>()
                .HasOne(t => t.Type)
                .WithMany(mc => mc.MedicalCenters)
                .HasForeignKey(mc => mc.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Physician>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Physician>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Physician>()
                .HasOne(p => p.MedicalCenter)
                .WithMany(a => a.Physicians)
                .HasForeignKey(p => p.MedicalCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Physician>()
                .HasOne(p => p.Speciality)
                .WithMany(s => s.Physicians)
                .HasForeignKey(p => p.SpecialityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Physician>()
                .HasMany(a => a.Appointments)
                .WithOne(p => p.Physician)
                .HasForeignKey(a => a.PhysicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Address>()
                .HasOne(a => a.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Patient>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
