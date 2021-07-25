using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Models.Physicians
{
    public class PhysicianFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; init; }

        [Required]
        public string Gender { get; init; }

        [Display(Name = "Medical Center")]
        public int MedicalCenterId { get; init; }

        [Range(ExaminationPriceMinValie, ExaminationPriceMaxValie)]
        public int ExaminationPrice { get; set; }

        [Url]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int SpecialityId { get; set; }

        public IEnumerable<PhysicianMedicalCentersServiceModel> MedicalCenters { get; set; }

        public IEnumerable<PhysicianSpecialityServiceModel> Specialities { get; set; }

        public bool IsWorkingWithChildren { get; set; }
    }
}
