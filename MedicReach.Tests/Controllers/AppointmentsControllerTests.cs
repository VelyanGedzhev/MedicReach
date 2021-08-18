using MedicReach.Controllers;
using MedicReach.Models.Appointments;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using System;
using Xunit;

namespace MedicReach.Tests.Controllers
{
    public class AppointmentsControllerTests
    {
        private const string MineAction = "Mine";

        [Theory]
        [InlineData("PhysicianId", "PatientId")]
        public void BookActionShouldBeForAuthorizedUsersAndReturnView(string physicianId, string patientId)
            => MyController<AppointmentsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Physicians.GetPhysician(physicianId),
                        Patients.GetPatient(patientId, null, null, TestUser.Identifier)))
                .Calling(c => c.Book(physicianId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AppointmentFormModel>()
                    .Passing(m => m
                        .PatientId == patientId
                        && m.PhysicianId == physicianId));

        [Theory]
        [InlineData("PhysicianId", "")]
        [InlineData("", "PatientId")]
        public void BookActionShouldBeForAuthorizedUsersAndReturBadRequestIfInvalidPhysicianOrPatient(string physicianId, string patientId)
            => MyController<AppointmentsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Physicians.GetPhysician(physicianId),
                        Patients.GetPatient(patientId, null, null, TestUser.Identifier)))
                .Calling(c => c.Book(physicianId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData("PhysicianId", "PatientId", "10:00")]
        public void BookPostActionShouldBeForAuthorizedUsersAndRedirectCorrectly(
            string physicianId,
            string patientId,
            string hour)
        {
            string currentDate = DateTime.UtcNow.AddDays(1).ToString("dd-MM-yyyy");

            MyController<AppointmentsController>
                           .Instance(instance => instance
                               .WithUser())
                           .Calling(c => c.Book(new AppointmentFormModel
                           {
                               PhysicianId = physicianId,
                               PatientId = patientId,
                               Date = currentDate,
                               Hour = hour
                           }))
                           .ShouldHave()
                           .ActionAttributes(a => a
                               .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName)
                               .RestrictingForHttpMethod(HttpMethod.Post))
                           .ValidModelState()
                           .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                           .AndAlso()
                           .ShouldReturn()
                           .RedirectToAction(MineAction);
        }

        [Theory]
        [InlineData("PhysicianId", "PatientId", "17-08-2021", "10:00")]
        public void BookPostActionShouldBeForAuthorizedUsersAndRedirectCorrectlyWhenPhysicianAlreadyHasAppointmentForThatDateAndHour(
            string physicianId,
            string patientId,
            string date,
            string hour)
            => MyController<AppointmentsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Appointments.GetAppointment(physicianId, patientId, date, hour)))
                .Calling(c => c.Book(new AppointmentFormModel
                {
                    PhysicianId = physicianId,
                    PatientId = patientId,
                    Date = date,
                    Hour = hour
                }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName)
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
                .AndAlso()
                .ShouldReturn()
                .View(view => view 
                    .WithModelOfType<AppointmentFormModel>()
                    .Passing(a => 
                        a.PatientId == patientId &&
                        a.PhysicianId == physicianId));

        [Theory]
        [InlineData("PhysicianId", "PatientId", "17-08-2021", "10:00", "AppointmentId")]
        public void ChangeStatusActionShouldBeForAuthorizedUsersAndRedirectCorrectly(
            string physicianId,
            string patientId,
            string date,
            string hour,
            string appointmentId)
            => MyController<AppointmentsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Appointments.GetAppointment(physicianId, patientId, date, hour, appointmentId)))
                .Calling(c => c.ChangeStatus(appointmentId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PhysicianRoleName))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(MineAction);
    }
}
