namespace MedicReach.Models.MedicalCenters
{
    public class MedicalCenterDetailsViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Address { get; init; }

        public string Type { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }

        public int PhysiciansCount { get; init; }
    }
}
