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
            this.CreateMap<MedicalCenterServiceModel, MedicalCenterFormModel>();

            this.CreateMap<MedicalCenter, MedicalCenterServiceModel>()
                .ForMember(
                    mc => mc.Type, 
                    cfg => cfg.MapFrom(mc => mc.MedicalCenterType.Name))
                .ForMember(
                    mc => mc.Address,
                    cfg => cfg.MapFrom(mc => $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City}"));

            this.CreateMap<PhysicianServiceModel, PhysicianFormModel>()
                .ForMember(
                    p => p.IsWorkingWithChildren,
                    cfg => cfg.MapFrom(p => p.IsWorkingWithChildren == "Yes"));
        }
    }
}
