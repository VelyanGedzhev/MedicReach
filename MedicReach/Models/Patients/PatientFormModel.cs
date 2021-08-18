using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Patient;

namespace MedicReach.Models.Patients
{
    public class PatientFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Full Name")]
        public string FullName { get; init; }

        [Required]
        public string Gender { get; init; }
    }
}
