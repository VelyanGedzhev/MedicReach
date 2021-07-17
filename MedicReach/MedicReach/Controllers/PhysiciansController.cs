using MedicReach.Data;
using MedicReach.Data.Models;
using MedicReach.Models.Physicians;
using MedicReach.Models.Physicians.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static MedicReach.Data.DataConstants.Speciality;
using static MedicReach.Data.DataConstants.Physician;

namespace MedicReach.Controllers
{
    public class PhysiciansController : Controller
    {
        private readonly MedicReachDbContext data;

        public PhysiciansController(MedicReachDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllPhysiciansQueryModel allPhysiciansQuery)
        {
            var physiciansQuery = this.data
                .Physicians
                .AsQueryable();

            if (!string.IsNullOrEmpty(allPhysiciansQuery.SearchTerm))
            {
                physiciansQuery = physiciansQuery
                    .Where(p =>
                    (p.FirstName.ToLower() + " " + p.LastName.ToLower()).Contains(allPhysiciansQuery.SearchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(allPhysiciansQuery.Speciality))
            {
                physiciansQuery = physiciansQuery
                    .Where(p => p.Speciality.Name == allPhysiciansQuery.Speciality);
            }

            physiciansQuery = allPhysiciansQuery.Sorting switch
            {
                PhysicianSorting.ExaminationPriceDesc => physiciansQuery.OrderByDescending(p => p.ExaminationPrice),
                PhysicianSorting.ExaminationPriceAsc => physiciansQuery.OrderBy(p => p.ExaminationPrice),
                PhysicianSorting.NameAsc => physiciansQuery.OrderBy(p => p.FirstName).ThenBy(p => p.LastName),
                PhysicianSorting.NameDesc => physiciansQuery.OrderByDescending(p => p.FirstName).ThenByDescending(p => p.LastName),
                PhysicianSorting.DateCreated or _ => physiciansQuery.OrderByDescending(p => p.Id)
            };

            var totalPhysicians = physiciansQuery.Count();

            var physicians = physiciansQuery
                .Skip((allPhysiciansQuery.CurrentPage - 1)  * AllPhysiciansQueryModel.PhysiciansPerPage)
                .Take(AllPhysiciansQueryModel.PhysiciansPerPage)
                .Select(p => new PhysicianListViewModel
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    Address = p.Address,
                    Speciality = p.Speciality.Name,
                    ImageUrl = p.ImageUrl,
                    ExaminationPrice = p.ExaminationPrice,
                    IsWorkingWithChildren = p.IsWorkingWithChildren ? "Yes" : "No"
                })
                .ToList();

            var physicianSpecialities = this.data
                .PhysicianSpecialities
                .Select(ps => ps.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            allPhysiciansQuery.Specialities = physicianSpecialities;
            allPhysiciansQuery.Physicians = physicians;
            allPhysiciansQuery.TotalPhysiciansCount = totalPhysicians;

            return View(allPhysiciansQuery);
        }

        public IActionResult Add() => View(new BecomePhysicianFormModel
        {
            Addresses = this.GetAddresses(),
            Specialities = this.GetSpecialities()
        });

        [HttpPost]
        public IActionResult Add(BecomePhysicianFormModel physicianModel)
        {
            if (!this.data.Addresses.Any(a => a.Id == physicianModel.AddressId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.AddressId), "Address does not exist.");
            }

            if (!this.data.PhysicianSpecialities.Any(ps => ps.Id == physicianModel.SpecialityId))
            {
                this.ModelState.AddModelError(nameof(physicianModel.SpecialityId), "Speciality does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                physicianModel.Addresses = this.GetAddresses();
                physicianModel.Specialities = this.GetSpecialities();

                return View(physicianModel);
            }

            var physicianImageUrl = string.Empty;

            if (physicianModel.Gender == GenderMale)
            {
                physicianImageUrl = DefaultMaleImageUrl;
            }
            else
            {
                physicianImageUrl = DefaultFemaleImageUrl;
            }

            var physician = new Physician
            {
                FirstName = physicianModel.FirstName,
                LastName = physicianModel.LastName,
                Gender = physicianModel.Gender,
                Email = physicianModel.Email,
                ExaminationPrice = physicianModel.ExaminationPrice,
                AddressId = physicianModel.AddressId,
                ImageUrl = physicianModel.ImageUrl ?? physicianImageUrl,
                SpecialityId = physicianModel.SpecialityId,
                IsWorkingWithChildren = physicianModel.IsWorkingWithChildren
            };

            this.data.Physicians.Add(physician);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }



        private IEnumerable<PhysicianAddressViewModel> GetAddresses()
            => this.data
                .Addresses
                .Select(c => new PhysicianAddressViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Number = c.Number,
                    City = c.City
                })
                .ToList();

        private IEnumerable<PhysicianSpecialityViewModel> GetSpecialities()
            => this.data
                .PhysicianSpecialities
                .Select(ps => new PhysicianSpecialityViewModel
                {
                    Id = ps.Id,
                    Name = ps.Name
                })
                .ToList();
    }
}
