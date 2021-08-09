using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.Reviews
{
    public class ReviewFormModel
    {
        [Required]
        public string PatientId { get; init; }

        [Required]
        public string PhysicianId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
