﻿using System.ComponentModel.DataAnnotations;

namespace MedicReach.Models.Appointments
{
    public class AppointmentFormModel
    {
        public string physicianId { get; set; }

        public string patientId { get; set; }

        [Required]
        //[ValidateDateString(ErrorMessage = GlobalConstants.ErrorMessages.DateTime)]
        public string Date { get; set; }

        [Required]
        //[ValidateTimeString(ErrorMessage = GlobalConstants.ErrorMessages.DateTime)]
        public string Hour { get; set; }
    }
}
