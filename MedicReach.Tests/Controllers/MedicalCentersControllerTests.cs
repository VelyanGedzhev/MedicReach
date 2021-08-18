using MedicReach.Areas.Admin;
using MedicReach.Controllers;
using MedicReach.Models.MedicalCenters;
using MedicReach.Services.MedicalCenters.Models;
using MedicReach.Tests.Data;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace MedicReach.Tests.Controllers
{
    public class MedicalCentersControllerTests
    {
        private const int MedicalCentersCount = 10;
        private const int MedicalCenterTypesCount = 4;

        [Theory]
        [InlineData("MedicalCenterId")]
        public void AllActionShouldReturnViewWithCorrectModel(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId)))
                .Calling(c => c.All(new AllMedicalCentersQueryModel { }))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllMedicalCentersQueryModel>()
                    .Passing(c => c.TotalMedicalCenters = MedicalCentersCount));

        [Fact]
        public void AddActionsShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel()
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        MedicalCenterTypes.GetTypes()))
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterFormModel>()
                    .Passing(c => 
                        c.MedicalCenterTypes.Count() == MedicalCenterTypesCount));

        [Theory]
        [InlineData("MedicalCenterId")]
        public void AddActionsShouldBeForAuthorizedUsersAndRedirectCorrectlyIdUserIsAlreadyCreator(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        MedicalCenters.GetMedicalCenter(medicalCenterId, null, TestUser.Identifier)))
                .Calling(c => c.Add())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests())
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(c => c.Index()));

        [Fact]
        public void AddPostActionShouldBeForAuthorizedUsersRedirectCorrectly()
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(
                        MedicalCenterTypes.GetMedicalCenterType(),
                        Cities.GetCity(null, 1)))
                .Calling(c =>
                    c.Add(new MedicalCenterFormModel
                    {
                        Name = "Hospital",
                        Address = "StreetName",
                        CityId  = 1,
                        CountryId = 1,
                        Description = "Description",
                        JoiningCode = "joiningCode",
                        TypeId = 1
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<PhysiciansController>(c => c.Become()));

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "", "Address", "Description", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "", "Description", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "Description", 1, 2)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "Description", 0, 1)]
        public void AddPostActionShouldBeForAuthorizedUsersAndReturnViewIfInvalidModelState(
            string medicalCenterId,
            string joiningCode,
            string creatorId,
            string medicalCenterName,
            string address,
            string description,
            int cityId,
            int countryId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode, creatorId)))
                .Calling(c =>
                    c.Add(new MedicalCenterFormModel
                    {
                        Name = medicalCenterName,
                        Address = address,
                        Description = description,
                        JoiningCode = joiningCode,
                        CityId = cityId,
                        CountryId = countryId
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterFormModel>());

        [Theory]
        [InlineData("MedicalCenterId")]
        public void DetailsActionShouldReturnViewWithCorrectModel(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId)))
                .Calling(c => c.Details(medicalCenterId))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterServiceModel>()
                    .Passing(mc => mc.Id == medicalCenterId));

        [Theory]
        [InlineData("MedicalCenterId")]
        public void MineActionShouldBeForAuthorizedUsersAndRedirectWithCorrectParameter(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, null, TestUser.Identifier)))
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PhysicianRoleName))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                        .To<MedicalCentersController>(c => c.Edit(medicalCenterId)));

        [Theory]
        [InlineData("MedicalCenterId")]
        public void MineActionShouldRedirectWithCorrectMessageIfUserDontOwnMedicalCenter(string medicalCenterId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId)))
                .Calling(c => c.Mine())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests(WebConstants.PhysicianRoleName))
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalErrorMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                        .To<HomeController>(c => c.Index()));

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier)]
        public void EditActionShouldBeForAuthorizedUsersAndReturnViewWithCorrectModel(
            string medicalCenterId, 
            string joiningCode, 
            string creatorId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode, creatorId)))
                .Calling(c => c.Edit(medicalCenterId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterFormModel>()
                    .Passing(mc =>
                        mc.JoiningCode == joiningCode));

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode")]
        public void EditActionShouldBeForAuthorizedUsersAndReturnViewWithCorrectModelIfUserIsAdmin(
            string medicalCenterId,
            string joiningCode)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser(TestUser.Identifier, AdminConstants.AdministratorRoleName, AdminConstants.AdministratorRoleName)
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode)))
                .Calling(c => c.Edit(medicalCenterId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterFormModel>()
                    .Passing(mc =>
                        mc.JoiningCode == joiningCode));

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode")]
        public void EditActionShouldBeForAuthorizedUsersAndReturnUnauthorizedIfUserIsNotCreatorOrAdmin(
            string medicalCenterId,
            string joiningCode)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode)))
                .Calling(c => c.Edit(medicalCenterId))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .AndAlso()
                .ShouldReturn()
                .Unauthorized();

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier)]
        public void EditPostActionShouldBeForAuthorizedUsersAndRedirectWithCorrectParameter(
            string medicalCenterId,
            string joiningCode,
            string creatorId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode, creatorId)))
                .Calling(c => 
                    c.Edit(medicalCenterId, new MedicalCenterFormModel
                    {
                        Name = "Hospital",
                        Address = "Address",
                        Description = "Description",
                        JoiningCode = joiningCode,
                        CityId = 1,
                        CountryId = 1
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .ValidModelState()
                .TempData(temp => temp
                    .ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<MedicalCentersController>(c => c.Details(medicalCenterId)));

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode")]
        public void EditPostActionShouldBeForAuthorizedUsersAndReturnUnauthorizedIfUserIsNotCreatorOrAdmin(
            string medicalCenterId,
            string joiningCode)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode)))
                .Calling(c =>
                    c.Edit(medicalCenterId, new MedicalCenterFormModel
                    {
                        Name = "Hospital",
                        Address = "Address",
                        Description = "Description",
                        JoiningCode = joiningCode,
                        CityId = 1,
                        CountryId = 1
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .AndAlso()
                .ShouldReturn()
                .Unauthorized();

        [Theory]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "", "Address", "Description", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "", "Description", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "", 1, 1)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "Description", 1, 2)]
        [InlineData("MedicalCenterId", "JoiningCode", TestUser.Identifier, "MedicalCenterName", "Address", "Description", 0, 1)]
        public void EditPostActionShouldBeForAuthorizedUsersAndReturnViewIfInvalidModelState(
            string medicalCenterId,
            string joiningCode,
            string creatorId,
            string medicalCenterName,
            string address,
            string description,
            int cityId,
            int countryId)
            => MyController<MedicalCentersController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(MedicalCenters.GetMedicalCenters(medicalCenterId, joiningCode, creatorId)))
                .Calling(c =>
                    c.Edit(medicalCenterId, new MedicalCenterFormModel
                    {
                        Name = medicalCenterName,
                        Address = address,
                        Description = description,
                        JoiningCode = joiningCode,
                        CityId = cityId,
                        CountryId = countryId
                    }))
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests($"{WebConstants.PhysicianRoleName},{AdminConstants.AdministratorRoleName}"))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<MedicalCenterFormModel>());
    }
}
