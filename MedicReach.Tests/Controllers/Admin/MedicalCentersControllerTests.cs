using MedicReach.Areas.Admin.Controllers;
using MedicReach.Areas.Admin.Models.MedicalCenters;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Controllers.Admin
{
    public class MedicalCentersControllerTests
    {
        private const int MedicalCentersCount = 10;

        [Theory]
        [InlineData("MedicalCenterId")]
        public void AllActionShouldReturnViewWithCorrectModel(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId)))
                .Calling(c => c.All(new AllMedicalCentersAdminQueryModel { }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllMedicalCentersAdminQueryModel>()
                    .Passing(c => c.TotalMedicalCenters = MedicalCentersCount));
    }
}
