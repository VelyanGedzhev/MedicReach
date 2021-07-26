using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.Patient;

namespace MedicReach.Data.Models
{
    public class Patient
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FullName { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<Review> Reviews { get; init; } = new List<Review>();
    }
}
