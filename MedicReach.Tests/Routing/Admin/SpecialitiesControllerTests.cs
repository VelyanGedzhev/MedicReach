using MedicReach.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedicReach.Tests.Routing.Admin
{
    public class SpecialitiesControllerTests
    {
        [Fact]
        public void AddActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Specialities/Add")
                .To<SpecialitiesController>(c => c.Add());

        [Fact]
        public void AddPostActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Admin/Specialities/Add")
                    .WithMethod(HttpMethod.Post))
                .To<SpecialitiesController>(c => c.Add());
    }
}
