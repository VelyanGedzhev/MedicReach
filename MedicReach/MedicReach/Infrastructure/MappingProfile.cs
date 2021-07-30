using AutoMapper;
using MedicReach.Data.Models;
using MedicReach.Models.MedicalCenters;
using MedicReach.Models.Physicians;
using MedicReach.Services.MedicalCenters.Models;
using MedicReach.Services.Physicians.Models;

namespace MedicReach.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Address, MedicalCenterAddressServiceModel>()
                .ForMember(
                    mc => mc.CountryCode,
                    cfg => cfg.MapFrom(mc => mc.Country.Alpha3Code));

            this.CreateMap<MedicalCenterServiceModel, MedicalCenterFormModel>();

            this.CreateMap<MedicalCenter, MedicalCenterServiceModel>()
                .ForMember(
                    mc => mc.Type, 
                    cfg => cfg.MapFrom(mc => mc.Type.Name))
                .ForMember(
                    mc => mc.Address,
                    cfg => cfg.MapFrom(mc => $"{mc.Address.Number} {mc.Address.Name}, {mc.Address.City}, {mc.Address.Country.Alpha3Code}"));

            this.CreateMap<MedicalCenter, PhysicianMedicalCentersServiceModel>()
                .ForMember(
                    a => a.Address,
                    cfg => cfg.MapFrom(mc => mc.Address.Name))
                .ForMember(
                    n => n.Number,
                    cfg => cfg.MapFrom(mc => mc.Address.Number))
                .ForMember(
                    c => c.City,
                    cfg => cfg.MapFrom(mc => mc.Address.City))
                .ForMember(
                    mc => mc.CountryCode,
                    cfg => cfg.MapFrom(mc => mc.Address.Country.Alpha3Code));

            this.CreateMap<MedicalCenterType, MedicalCenterTypeServiceModel>();

            this.CreateMap<PhysicianServiceModel, PhysicianFormModel>()
                .ForMember(
                    p => p.IsWorkingWithChildren,
                    cfg => cfg.MapFrom(p => p.IsWorkingWithChildren == "Yes"));

            this.CreateMap<Physician, PhysicianServiceModel>()
                .ForMember(
                    p => p.FullName,
                    cfg => cfg.MapFrom(p => p.User.FullName))
                .ForMember(
                    p => p.Speciality,
                    cfg => cfg.MapFrom(p => p.Speciality.Name))
                .ForMember(
                    p => p.Address,
                    cfg => cfg.MapFrom(p => $"{p.MedicalCenter.Address.Number} {p.MedicalCenter.Address.Name}, {p.MedicalCenter.Address.City}, {p.MedicalCenter.Address.Country.Alpha3Code}"))
                .ForMember(
                    p => p.IsWorkingWithChildren,
                    cfg => cfg.MapFrom(p => p.IsWorkingWithChildren == true ? "Yes" : "No"));

            this.CreateMap<PhysicianSpeciality, PhysicianSpecialityServiceModel>();
        }
    }
}
