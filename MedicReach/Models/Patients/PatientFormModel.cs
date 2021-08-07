using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Patient;

namespace MedicReach.Models.Patients
{
    public class PatientFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FullName { get; init; }

        [Required]
        public string Gender { get; init; }
    }
}
