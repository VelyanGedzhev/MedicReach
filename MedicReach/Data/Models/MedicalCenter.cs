using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.MedicalCenter;

namespace MedicReach.Data.Models
{
    public class MedicalCenter
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(AddressNameMaxLength)]
        public string Address { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public string CreatorId { get; init; }

        [Required]
        [MaxLength(JoiningCodeMaxLength)]
        public string JoiningCode { get; set; }

        public int TypeId { get; set; }

        public MedicalCenterType Type { get; set; }

        public IEnumerable<Physician> Physicians { get; init; } = new List<Physician>();
    }
}
