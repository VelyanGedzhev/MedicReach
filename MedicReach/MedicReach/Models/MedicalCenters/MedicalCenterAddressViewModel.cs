namespace MedicReach.Models.MedicalCenters
{
    public class MedicalCenterAddressViewModel
    {
        public int Id { get; init; }

        public string AddressName { get; init; }

        public int AddressNumber { get; init; }

        public string ImageUrl { get; init; }

        public string City { get; init; }

        public string CountryCode { get; init; }
    }
}
