using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Data.Models
{
    public class MedicalCenter
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; init; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public IEnumerable<Physician> Physicians { get; init; } = new List<Physician>();
    }
}
