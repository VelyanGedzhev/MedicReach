using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Address;

namespace MedicReach.Data.Models
{
    public class Address
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int Number { get; set; }

        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; }

        public IEnumerable<MedicalCenter> MedicalCenters { get; init; } = new List<MedicalCenter>();

        public IEnumerable<Physician> Physicians { get; init; } = new List<Physician>();
    }
}
