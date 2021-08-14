using Xunit;
using MyTested.AspNetCore.Mvc;
using MedicReach.Controllers;
using MedicReach.Models.Physicians;
using MedicReach.Data.Models;
using System.Linq;

namespace MedicReach.Tests.Controllers
{
    public class PhysiciansControllerTests
    {
        [Fact]
        public void BecomeShouldBeForAuthorizedUsersAndReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithPath("/Physicians/Become")
                    .WithUser())
                .To<PhysiciansController>(c => c.Become())
                .Which()
                .ShouldHave()
                .ActionAttributes(attirbutes => attirbutes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Fact]
        public void BecomeShouldBeForAuthorizedUsers()
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Become())
                .ShouldHave()
                .ActionAttributes(a => a
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void BecomeShouldReturnView()
            => MyController<PhysiciansController>
                .Instance(instance => instance
                    .WithUser())
                .Calling(c => c.Become())
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Ivan Petrov", "Male", "b4d9d520-b735-42e5-9b9a-3d1fdc7e55bf", "MedicalCenter", 50, 1, "PP1234599", false)]
        public void BecomePostShouldBeForAuthorizedUsersAndRedirectCorrectly(
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
                    .WithUser())
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
                            p.Gender == gender)));
    }
}
