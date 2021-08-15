using Microsoft.AspNetCore.Identity;

namespace MedicReach.Tests.Data
{
    public static class UserRoles
    {
        public static IdentityRole GetRole(string name)
        {
            return new IdentityRole
            { 
                Name = name,
                NormalizedName = name.ToUpper()
            };
        }
    }
}
