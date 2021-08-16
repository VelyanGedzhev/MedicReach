using MedicReach.Controllers;
using MedicReach.Models.Reviews;
using MedicReach.Services.Appointments.Models;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Controllers
{
    public class ReviewsControllerTests
    {
        [Theory]
        [InlineData("PhysicianId", "PatientId", "17-08-2021", "10:00", "AppointmentId")]
        public void WriteActionShouldBeForAuthorizedPatientUserAndShouldRedirectWithCorrectModel(
            string physicianId,
            string patientId,
            string date,
            string hour,
            string appointmentId)
            => MyController<ReviewsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Appointments.GetAppointment(physicianId, patientId, date, hour, appointmentId)))
                .Calling(c => c.Write(appointmentId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>()
                    .Passing(r =>
                        r.AppointmentId == appointmentId &&
                        r.PhysicianId == physicianId &&
                        r.PatientId == patientId));

        [Theory]
        [InlineData("PhysicianId", "PatientId", "17-08-2021", "10:00", "AppointmentId", 5)]
        public void WritePostActionShouldBeForAuthorizedPatientUserAndShouldRedirectWithCorrectModel(
            string physicianId,
            string patientId,
            string date,
            string hour,
            string appointmentId,
            int rating)
            => MyController<ReviewsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Appointments.GetAppointment(physicianId, patientId, date, hour, appointmentId)))
                .Calling(c => 
                    c.Write(new ReviewFormModel 
                    {
                        PhysicianId = physicianId,
                        PatientId = patientId,
                        AppointmentId = appointmentId,
                        Rating = rating,
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName)
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<AppointmentsController>(c => c.Mine(With.Any<AllMyAppointmentsQueryServiceModel>())));
    }
}
