using AutoMapper;
using MedicReach.Data.Models;
using MedicReach.Models.MedicalCenters;
using MedicReach.Models.Patients;
using MedicReach.Models.Physicians;
using MedicReach.Services.Appointments.Models;
using MedicReach.Services.MedicalCenters.Models;
using MedicReach.Services.Patients.Models;
using MedicReach.Services.Physicians.Models;
using MedicReach.Services.Reviews.Models;

namespace MedicReach.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<MedicalCenterServiceModel, MedicalCenterFormModel>();

            this.CreateMap<MedicalCenter, MedicalCenterServiceModel>()
                .ForMember(
                    mc => mc.Type,
                    cfg => cfg.MapFrom(mc => mc.Type.Name))
                .ForMember(
                    mc => mc.City,
                    cfg => cfg.MapFrom(c => c.City.Name))
                .ForMember(
                    mc => mc.Country,
                    cfg => cfg.MapFrom(c => c.Country.Name));

            this.CreateMap<MedicalCenter, PhysicianMedicalCentersServiceModel>()
                .ForMember(
                    c => c.City,
                    cfg => cfg.MapFrom(mc => mc.City.Name))
                .ForMember(
                    mc => mc.Country,
                    cfg => cfg.MapFrom(mc => mc.Country.Alpha3Code));

            this.CreateMap<MedicalCenterType, MedicalCenterTypeServiceModel>();

            this.CreateMap<PhysicianServiceModel, PhysicianFormModel>()
                .ForMember(
                    p => p.IsWorkingWithChildren,
                    cfg => cfg.MapFrom(p => p.IsWorkingWithChildren == "Yes"));

            this.CreateMap<Physician, PhysicianServiceModel>()
                .ForMember(
                    p => p.Speciality,
                    cfg => cfg.MapFrom(p => p.Speciality.Name))
                .ForMember(
                    p => p.Address,
                    cfg => cfg.MapFrom(p => $" {p.MedicalCenter.Address}, {p.MedicalCenter.City.Name}, {p.MedicalCenter.Country.Name}"))
                .ForMember(
                    p => p.IsWorkingWithChildren,
                    cfg => cfg.MapFrom(p => p.IsWorkingWithChildren == true ? "Yes" : "No"));

            this.CreateMap<PhysicianSpeciality, PhysicianSpecialityServiceModel>();

            this.CreateMap<Appointment, AppointmentServiceModel>()
                .ForMember(
                    a => a.PhysicianName,
                    cfg => cfg.MapFrom(mc => mc.Physician.FullName))
                .ForMember(
                    a => a.PatientName,
                    cfg => cfg.MapFrom(mc => mc.Patient.FullName));

            this.CreateMap<Patient, PatientServiceModel>();

            this.CreateMap<PatientServiceModel, PatientFormModel>();

            this.CreateMap<Review, ReviewServiceModel>();
        }
    }
}
