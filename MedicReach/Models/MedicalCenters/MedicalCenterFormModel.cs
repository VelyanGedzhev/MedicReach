using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Models.MedicalCenters
{
    public class MedicalCenterFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(AddressNameMaxLength, MinimumLength = AdressNameMinLength)]
        public string Address { get; set; }

        [Display(Name = "Type")]
        public int TypeId { get; init; }

        [Display(Name = "City")]
        public int CityId { get; init; }

        [Display(Name = "Country")]
        public int CountryId { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; init; }

        [Required]
        [StringLength(JoiningCodeMaxLength, MinimumLength = JoiningCodeMinLength)]
        [Display(Name = "Joining Code")]
        public string JoiningCode { get; init; }

        [Url]
        [Display(Name = "Image")]
        public string ImageUrl { get; init; }

        public IEnumerable<CityServiceModel> Cities { get; set; }

        public IEnumerable<CountryServiceModel> Countries { get; set; }

        public IEnumerable<MedicalCenterTypeServiceModel> MedicalCenterTypes { get; set; }
    }
}
