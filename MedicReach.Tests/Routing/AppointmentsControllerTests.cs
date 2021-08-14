using MedicReach.Controllers;
using MedicReach.Models.Appointments;
using MedicReach.Services.Appointments.Models;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class AppointmentsControllerTests
    {
        [Fact]
        public void BookActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Appointments/Book")
                .To<AppointmentsController>(c => c.Book(With.Any<string>()));

        [Fact]
        public void BookPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Appointments/Book")
                    .WithMethod(HttpMethod.Post))
                .To<AppointmentsController>(c => c.Book(With.Any<AppointmentFormModel>()));

        [Fact]
        public void MineActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Appointments/Mine")
                .To<AppointmentsController>(c => c.Mine(With.Any<AllMyAppointmentsQueryServiceModel>()));

        [Fact]
        public void ChangeStatusActionMustBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Appointments/ChangeStatus")
                .To<AppointmentsController>(c => c.ChangeStatus(With.Any<string>()));
    }
}
