namespace MedicReach.Services.Physicians.Models
{
    public class PhysicianMedicalCentersServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string  Address { get; init; }

        public int AddressNumber { get; init; }

        public string  City { get; init; }

        public string  CountryCode { get; init; }
    }
}
