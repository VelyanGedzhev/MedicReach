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

        public int AddressId { get; set; }

        public Address Address { get; init; }

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
