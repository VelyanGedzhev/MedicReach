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

        public static IEnumerable<IdentityUser> GetUsers(string userId)
        {
            var users = Enumerable.Range(0, 2).Select(u => new IdentityUser
            {
            })
            .ToList();

            var user = new IdentityUser
            {
                Id = userId
            };

            users.Add(user);

            return users;
        }
    }
}
