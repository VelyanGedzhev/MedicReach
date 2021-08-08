using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Speciality;

namespace MedicReach.Areas.Admin.Models.Specialities
{
    public class SpecialityFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }
    }
}
