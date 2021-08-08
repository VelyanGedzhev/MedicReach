using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.Appointments
{
    public class AppointmentFormModel
    {
        public string PhysicianId { get; init; }

        public string PatientId { get; init; }

        [Required]
        public string Date { get; init; }

        [Required]
        public string Hour { get; init; }

        public bool IsApproved { get; init; }
    }
}
