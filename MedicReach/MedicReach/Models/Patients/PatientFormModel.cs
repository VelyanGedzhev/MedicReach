using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.Patients
{
    public class PatientFormModel
    {
        [Required]
        public string Gender { get; init; }
    }
}
