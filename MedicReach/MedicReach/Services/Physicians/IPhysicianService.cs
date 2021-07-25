﻿using MedicReach.Models.Physicians.Enums;
using MedicReach.Services.Physicians.Models;
using System.Collections.Generic;

namespace MedicReach.Services.Physicians
{
    public interface IPhysicianService
    {
        PhysicanQueryServiceModel All(
            string Speciality,
            string MedicalCenter,
            string searchTerm,
            PhysicianSorting sorting,
            int currentPage,
            int physiciansPerPage);

        IEnumerable<string> AllMedicalCenters();

        IEnumerable<string> AllSpecialities();

        bool IsPhysician(string userId);

        IEnumerable<PhysicianSpecialityServiceModel> GetSpecialities();

        IEnumerable<PhysicianMedicalCentersServiceModel> GetMedicalCenters();

        PhysicianServiceModel Details(int physicianId);

        void Create(
            string firstName,
            string lastName,
            string gender,
            int examinationPrice,
            int medicalCenterId,
            string imageUrl,
            int specialityId,
            bool IsWorkingWithChildren,
            string UserId);

        string PrepareDefaultImage(string gender);
    }
}
