namespace MedicReach.Services.MedicalCenters.Models
{
    public class MedicalCenterServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int AddressId { get; init; }

        public string Address { get; init; }

        public int TypeId { get; init; }

        public string Type { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }

        public int PhysiciansCount { get; init; }
    }
}
