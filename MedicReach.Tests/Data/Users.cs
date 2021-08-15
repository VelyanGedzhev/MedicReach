using Microsoft.AspNetCore.Identity;

namespace MedicReach.Tests.Data
{
    public static class Users
    {
        public static IdentityUser GetUser(string userId)
        {
            return new IdentityUser
            {
                Id = userId
            };
        }
    }
}
