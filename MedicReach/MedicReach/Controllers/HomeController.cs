using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicReach.Data;
using MedicReach.Models;
using MedicReach.Services.MedicalCenters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace MedicReach.Controllers
{
    public class HomeController : Controller
    {
        private readonly MedicReachDbContext data;
        private readonly IMapper mapper;

        public HomeController(
            MedicReachDbContext data, 
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var medicalCenters = this.data
                .MedicalCenters
                .Where(x => x.Physicians.Any(p => p.IsApproved))
                .ProjectTo<MedicalCenterServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

            return View(medicalCenters);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
