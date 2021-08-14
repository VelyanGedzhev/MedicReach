
using MedicReach.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MedicReach.Tests.Routing
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Index")
                .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorActionShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());
    }
}
