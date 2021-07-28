using Microsoft.AspNetCore.Mvc;


namespace MedicReach.Areas.Admin.Controllers
{

    public class PhysiciansController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
