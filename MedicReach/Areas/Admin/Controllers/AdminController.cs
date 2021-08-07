using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MedicReach.Areas.Admin.AdminConstants;

namespace MedicReach.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {
    }
}
