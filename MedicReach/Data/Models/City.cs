using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.City;

namespace MedicReach.Data.Models
{
    public class City
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        public int CountryId { get; init; }

        public Country Country { get; init; }

        public IEnumerable<MedicalCenter> MedicalCenters { get; init; } = new List<MedicalCenter>();
    }
}
