using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.County;

namespace MedicReach.Data.Models
{
    public class Country
    {
        public int Id { get; init; }
        
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(Alpha3CodeMaxLength)]
        public string Alpha3Code { get; init; }

        public IEnumerable<Address> Addresses { get; init; } = new List<Address>();
    }
}
