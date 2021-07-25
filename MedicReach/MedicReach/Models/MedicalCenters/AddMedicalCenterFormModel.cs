using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Models.MedicalCenters
{
    public class AddMedicalCenterFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Display(Name = "Address")]
        public int AddressId { get; init; }

        [Display(Name = "Type")]
        public int TypeId { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; init; }

        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        public IEnumerable<MedicalCenterAddressServiceModel> Addresses { get; set; }

        public IEnumerable<MedicalCenterTypeServiceModel> MedicalCenterTypes { get; set; }
    }
}
