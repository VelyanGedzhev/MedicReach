using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.City;

namespace MedicReach.Areas.Admin.Models.Cities
{
    public class CityFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Display(Name = "Country")]
        public int CountryId { get; init; }

        public IEnumerable<CountryServiceModel> Countries;
    }
}
