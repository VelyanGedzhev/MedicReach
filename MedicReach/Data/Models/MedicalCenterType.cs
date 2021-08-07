using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenterType;

namespace MedicReach.Data.Models
{
    public class MedicalCenterType
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; init; }

        public IEnumerable<MedicalCenter> MedicalCenters { get; init; } = new List<MedicalCenter>();
    }
}
