using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MedicReach.Data.DataConstants.User;

namespace MedicReach.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }
    }
}
