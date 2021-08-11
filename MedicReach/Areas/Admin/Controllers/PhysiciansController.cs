using MedicReach.Areas.Admin.Models.Physicians;
using MedicReach.Services.Physicians;
using Microsoft.AspNetCore.Mvc;


namespace MedicReach.Areas.Admin.Controllers
{
    public class PhysiciansController : AdminController
    {
        private readonly IPhysicianService physicians;

        public PhysiciansController(IPhysicianService physicians) 
            => this.physicians = physicians;

        public IActionResult All(AllPhysiciansAdminQueryModel query)
        {
            var queryResult = this.physicians
                .All(
                    null,
                    null,
                    null,
                    query.Sorting,
                    query.CurrentPage,
                    AllPhysiciansAdminQueryModel.PhysiciansPerPage,
                    approved: false
                    );

            query.Physicians = queryResult.Physicians;
            query.TotalPhysicians = queryResult.TotalPhysicians;

            return View(query);
        }

        public IActionResult Approve(string physicianId)
        {
            this.physicians.ChangeApprovalStatus(physicianId);

            return RedirectToAction(nameof(All));
        }
    }
}
