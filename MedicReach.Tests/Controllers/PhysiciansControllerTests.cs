using Xunit;
using MyTested.AspNetCore.Mvc;
using MedicReach.Controllers;
using MedicReach.Models.Physicians;
using MedicReach.Data.Models;
using System.Linq;
using MedicReach.Services.Physicians.Models;
using MedicReach.Tests.Data;

namespace MedicReach.Tests.Controllers
{
    public class PhysiciansControllerTests
    {
        [Fact]
        public void BecomeActionShouldBeForAuthorizedUsersAndReturnView()
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Ivan Petrov", "Male", "MedicalCenterId", "MedicalCenter", 50, 1, "PP1234599", false)]
        [InlineData("Emily Blunt", "Female", "MedicalCenterId", "MedicalCenter", 70, 1, "PP1234588", false)]
        public void BecomePostActionShouldBeForAuthorizedUsersAndRedirectCorrectly(
            string fullName,
            string gender,
            string medicalCenterId,
            string joiningCode,
            int examinationPrice,
            int specialityId,
            string practicePermissionNumber,
            bool IsApproved)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                    MedicalCenters.GetMedicalCenter(medicalCenterId, joiningCode),
                    Specialities.GetSpeciality(specialityId),
                    Users.GetUser(TestUser.Identifier),
                    UserRoles.GetRole("Physician")))
                .Calling(c => c.Become(new PhysicianFormModel
                {
                    FullName = fullName,
                    Gender = gender,
                    MedicalCenterId = medicalCenterId,
                    JoiningCode = joiningCode,
                    ExaminationPrice = examinationPrice,
                    SpecialityId = specialityId,
                    PracticePermissionNumber = practicePermissionNumber,
                    IsApproved = IsApproved
                }))
                .ShouldHave()
                .ValidModelState()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .Data(data => data
                    .WithSet<Physician>(physicians => physicians
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
        [InlineData("PhyscianId", "UserId")]
        public void DetailsActionShouldReturnViewWithCorrectModel(string physicianId, string userId)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithData(Physicians.GetPhysicians(physicianId, userId)))
                .Calling(c => c.Details(physicianId))
                .ShouldReturn()
                .View(view => view  
                    .WithModelOfType<PhysicianServiceModel>()
                    .Passing(model => model.Id == physicianId));

        [Theory]
        [InlineData("PhysicianId")]
        public void EditActionShouldReturnView(string physicianId)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(Physicians.GetPhysicians(physicianId)))
                .Calling(c => c.Edit(physicianId))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PhysicianFormModel>());

        [Theory]
        [InlineData("PhysicianId", "Ivan Petrov", "Male", "MedicalCenterId", "MedicalCenter", 50, 1, "PP1234599", false)]
        public void EditPostActionShouldRedirectToActionWithCorrectModel(
            string physicianId, 
            string fullName,
            string gender,
            string medicalCenterId,
            string joiningCode,
            int examinationPrice,
            int specialityId,
            string practicePermissionNumber,
            bool IsApproved)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Physicians.GetPhysicians(physicianId)))
                .Calling(c => c.Edit(physicianId, new PhysicianFormModel
                {
                    FullName = fullName,
                    Gender = gender,
                    MedicalCenterId = medicalCenterId,
                    JoiningCode = joiningCode,
                    ExaminationPrice = examinationPrice,
                    SpecialityId = specialityId,
                    PracticePermissionNumber = practicePermissionNumber,
                    IsApproved = IsApproved
                }))
                .ShouldHave()
                .ValidModelState()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .Data(data => data
                    .WithSet<Physician>(physicians => physicians
                        .Any(p =>
                            p.FullName == fullName)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<PhysiciansController>(c => c.Details(physicianId)));

        [Theory]
        [InlineData("PhysicianId")]
        public void MineActionShouldRedirectToActionWithCorrectModel(string physicianId)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Users.GetUser(TestUser.Identifier),
                        Physicians.GetPhysician(physicianId, TestUser.Identifier)))
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<PhysiciansController>(c => c.Edit(physicianId)));

    }
}

