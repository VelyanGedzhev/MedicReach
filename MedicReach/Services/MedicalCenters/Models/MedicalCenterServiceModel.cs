namespace MedicReach.Services.MedicalCenters.Models
{
    public class MedicalCenterServiceModel
    {
        public string Id { get; init; }

        public string Name { get; init; }

        public string Address { get; init; }

        public int TypeId { get; init; }

        public string Type { get; init; }

        public int CityId { get; init; }

        public string City { get; init; }

        public int CountryId { get; init; }

        public string Country { get; init; }

        public string Description { get; init; }

        public string JoiningCode { get; init; }

        public string ImageUrl { get; init; }

        public int PhysiciansCount { get; init; }
    }
}
