using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Data.Models
{
    public class Physician
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength)]
        public string FirstName { get; init; }

        [Required]
        [StringLength(NameMaxLength)]
        public string LastName { get; init; }

        [Required]
        public string Gender { get; init; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        public int ExaminationPrice { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public int SpecialityId { get; set; }

        public PhysicianSpeciality Speciality { get; set; }

        public bool IsWorkingWithChildren { get; set; }

        //[Required]
        //public string UserId { get; set; }

        //public IdentityUser User { get; set; }
    }
}
