using MedicReach.Services.Appointments.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicReach.Services.Appointments.Models
{
    public class AllMyAppointmentsQueryServiceModel
    {
        public const int AppointmentsPerPage = 5;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Find by Name")]
        public string SearchTerm { get; set; }

        [Display(Name = "Sort By:")]
        public AppointmentSorting Sorting { get; set; }

        public IEnumerable<AppointmentServiceModel> Appointments { get; set; }

        public int TotalAppointments { get; set; }
    }
}
