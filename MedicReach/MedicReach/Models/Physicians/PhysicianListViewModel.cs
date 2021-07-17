using MedicReach.Data.Models;

namespace MedicReach.Models.Physicians
{
    public class PhysicianListViewModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Gender { get; init; }

        public Address Address { get; init; }

        public int ExaminationPrice { get; init; }

        public string ImageUrl { get; init; }

        public string Speciality { get; init; }

        public string IsWorkingWithChildren { get; init; }
    }
}
