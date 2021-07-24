﻿using MedicReach.Data;
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

        public HomeController(MedicReachDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var medicalCenters = this.data
                .MedicalCenters
                .Select(mc => new MedicalCenterServiceModel
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Type = mc.MedicalCenterType.Name,
                    Address = $"{mc.Address.Number} {mc.Address.Name} {mc.Address.City}",
                    Description = mc.Description,
                    ImageUrl = mc.ImageUrl
                })
                .Take(3)
                .ToList();

            return View(medicalCenters);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
