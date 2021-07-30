using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedicReach.Data.Models
{
    public class Patient
    {
        public int Id { get; init; }

        [Required]
        public string Gender { get; init; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public IEnumerable<Appointment> Appointments { get; init; } = new List<Appointment>();

        public IEnumerable<Review> Reviews { get; init; } = new List<Review>();
    }
}
