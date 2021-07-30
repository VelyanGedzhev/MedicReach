using MedicReach.Data;
using System.Linq;
using static MedicReach.Data.DataConstants.User;

namespace MedicReach.Services.Users
{
    public class UserService : IUserService
    {
        private readonly MedicReachDbContext data;

        public UserService(MedicReachDbContext data)
        {
            this.data = data;
        }

        public bool IsPhysician(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.Type == TypePhysician);
    }
}
