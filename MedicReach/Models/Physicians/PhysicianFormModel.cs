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
        [Display(Name = "Full Name")]
        public string FullName { get; init; }

        [Required]
        public string Gender { get; init; }

        [Required]
        [Display(Name = "Medical Center")]
        public string MedicalCenterId { get; init; }

        [Required]
        [Display(Name = "Medical Center Joining Code")]
        public string JoiningCode { get; set; }

        [Range(ExaminationPriceMinValue, ExaminationPriceMaxValue)]
        [Display(Name = "Examination Price €")]
        public int ExaminationPrice { get; set; }

        [Url]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Speciality")]
        public int SpecialityId { get; set; }

        public IEnumerable<PhysicianMedicalCentersServiceModel> MedicalCenters { get; set; }

        public IEnumerable<PhysicianSpecialityServiceModel> Specialities { get; set; }

        [Display(Name = "Works with children")]
        public bool IsWorkingWithChildren { get; set; }

        [Required]
        [StringLength(PermissionPracticeMaxLength, MinimumLength = PermissionPracticeMinLength)]
        [Display(Name = "Practice Permission Number")]
        public string PracticePermissionNumber { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }
    }
}
