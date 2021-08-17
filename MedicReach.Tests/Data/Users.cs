using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

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
