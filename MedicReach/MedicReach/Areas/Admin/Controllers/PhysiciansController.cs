using MedicReach.Services.Physicians;
using Microsoft.AspNetCore.Mvc;


namespace MedicReach.Areas.Admin.Controllers
{
    public class PhysiciansController : AdminController
    {
        private readonly IPhysicianService physicians;

        public PhysiciansController(IPhysicianService physicians) 
            => this.physicians = physicians;

        public IActionResult All()
        {
            var physicians = this.physicians
                .All(approved: false)
                .Physicians;

            return View(physicians);
        }

        public IActionResult Approve(string physicianId)
        {
            this.physicians.ChangeApprovalStatus(physicianId);

            return RedirectToAction(nameof(All));
        }
    }
}
