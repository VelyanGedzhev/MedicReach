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
        private const int AllApprovedPhysiciansCount = 4;
        private const int PhysiciansWithAName = 1;

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
                    UserRoles.GetRole(WebConstants.PhysicianRoleName)))
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
        [InlineData("", "Male", "MedicalCenterId", "MedicalCenter", 50, 1, "PP1234599", false)]
        [InlineData("Ivan Petrov", "", "MedicalCenterId", "MedicalCenter", 50, 1, "PP1234599", false)]
        [InlineData("Ivan Petrov", "Male", "MedicalCenterId", "", 50, 1, "PP1234599", false)]
        [InlineData("Emily Blunt", "Female", "", "MedicalCenter", 70, 1, "PP1234588", false)]
        [InlineData("Emily Blunt", "Female", "MedicalCenterId", "MedicalCenter", -1, 1, "PP1234588", false)]
        [InlineData("Emily Blunt", "Female", "MedicalCenterId", "MedicalCenter", 70, 1, "", false)]
        [InlineData("Emily Blunt", "Female", "MedicalCenterId", "MedicalCenter", 70, 0, "PP1234588", false)]
        public void BecomePostActionShouldReturnViewWhenModelStateInvalid(
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
                    UserRoles.GetRole(WebConstants.PhysicianRoleName)))
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
                .InvalidModelState()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PhysicianFormModel>());

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
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{Areas.Admin.AdminConstants.AdministratorRoleName}"))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<PhysicianFormModel>());

        [Theory]
        [InlineData("PhysicianId", "Ivan Petrov", "Male", "MedicalCenterId", "MedicalCenter", 50, 1, "PP1234599", false)]
        public void EditPostActionShouldRedirectToActionWithCorrectParameter(
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
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{Areas.Admin.AdminConstants.AdministratorRoleName}")
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
        public void MineActionShouldRedirectToActionWithCorrectParameter(string physicianId)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        Users.GetUser(TestUser.Identifier),
                        Physicians.GetPhysician(physicianId, TestUser.Identifier)))
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PhysicianRoleName))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<PhysiciansController>(c => c.Edit(physicianId)));

        [Fact]
        public void AllActionShouldReturnViewWithAllCurrentlyApprovedPhysicians()
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithData(Physicians.GetPhysicians(TestUser.Identifier, null, true)))
                .Calling(c => 
                    c.All(new AllPhysiciansQueryModel
                    {
                    }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllPhysiciansQueryModel>()
                    .Passing(c => c.TotalPhysicians == AllApprovedPhysiciansCount));

        [Theory]
        [InlineData("PhysicianId", "John Johnson")]
        public void AllActionShouldReturnViewWithCorrectModelWhenSearchedByName(string physicianId, string fullName)
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithData(Physicians.GetPhysician(physicianId, null, fullName)))
                .Calling(c =>
                    c.All(new AllPhysiciansQueryModel
                    {
                        SearchTerm = fullName
                    }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllPhysiciansQueryModel>()
                    .Passing(c => 
                        c.Physicians.Any(p => p.FullName == fullName) && 
                        c.Physicians.Count() == PhysiciansWithAName));
    }
}

