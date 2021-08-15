using MedicReach.Controllers;
using MedicReach.Models.Patients;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class PatientsControllerTests
    {
        [Fact]
        public void BecomeActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Patients/Become")
                .To<PatientsController>(c => c.Become());


        [Fact]
        public void BecomePostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Patients/Become")
                    .WithMethod(HttpMethod.Post))
                .To<PatientsController>(c => c.Become(With.Any<PatientFormModel>()));

        [Fact]
        public void EditActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Patients/Edit")
                .To<PatientsController>(c => c.Edit());

        [Fact]
        public void EditPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Patients/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<PatientsController>(c => c.Edit(With.Any<PatientFormModel>()));
    }
}
