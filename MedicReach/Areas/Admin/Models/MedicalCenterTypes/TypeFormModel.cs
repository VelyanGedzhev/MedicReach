using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenterType;

namespace MedicReach.Areas.Admin.Models.MedicalCenterTypes
{
    public class TypeFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }
    }
}
