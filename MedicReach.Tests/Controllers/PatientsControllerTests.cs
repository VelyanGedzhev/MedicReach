using MedicReach.Controllers;
using MedicReach.Data.Models;
using MedicReach.Models.Patients;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace MedicReach.Tests.Controllers
{
    public class PatientsControllerTests
    {
        [Fact]
        public void BecomeActionShouldBeForAuthorizedUsersAndReturnView()
            => MyController<PatientsController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view 
                    .WithModelOfType<PatientFormModel>());

        [Theory]
        [InlineData("Ivan Petrov", "Male")]
        [InlineData("Emily Blunt", "Female")]
        public void BecomePostActionShouldBeForAuthorizedUsersAndRedirectCorrectly(
            string fullName,
            string gender)
            => MyController<PatientsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                    Users.GetUser(TestUser.Identifier),
                    UserRoles.GetRole(WebConstants.PatientRoleName)))
                .Calling(c => c.Become(new PatientFormModel
                {
                    FullName = fullName,
                    Gender = gender
                }))
                .ShouldHave()
                .ValidModelState()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Patient>(patients => patients
                        .Any(p =>
                            p.FullName == fullName &&
                            p.Gender == gender)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(c => c.Index()));

        [Theory]
        [InlineData("PatientId", "Ivan Petrov", "Male")]
        [InlineData("PatientId", "Emily Blunt", "Female")]
        public void EditActionShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel(
            string patientId, 
            string fullName, 
            string gender)
            => MyController<PatientsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Patients.GetPatients(
                        patientId, 
                        fullName, 
                        gender,
                        TestUser.Identifier)))
                .Calling(c => c.Edit())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName))
                .Data(data => data
                    .WithSet<Patient>(patient => patient
                        .Any(p =>
                            p.FullName == fullName
                            && p.Gender == gender)))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PatientFormModel>());

        [Theory]
        [InlineData("PatientId", "Ivan Petrov", "Male")]
        [InlineData("PatientId", "Emily Blunt", "Female")]
        public void EditPostActionShouldBeForAuthorizedUsersAndRedirectCorrectly(
            string patientId,
            string fullName,
            string gender)
            => MyController<PatientsController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Patients.GetPatients(
                            patientId, 
                            fullName, 
                            gender,
                            TestUser.Identifier)))
                .Calling(c => c.Edit(new PatientFormModel
                {
                    FullName = fullName,
                    Gender = gender
                }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PatientRoleName)
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .Data(data => data
                    .WithSet<Patient>(patient => patient
                        .Any(p =>
                            p.FullName == fullName &&
                            p.Gender == gender)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(c => c.Index()));
    }
}
