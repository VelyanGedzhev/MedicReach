﻿using MedicReach.Models.MedicalCenters.Enums;
using MedicReach.Services.MedicalCenters.Models;
using System.Collections.Generic;

namespace MedicReach.Services.MedicalCenters
{
    public interface IMedicalCenterService
    {
        void Create(
             string name,
             int addressId,
             int medicalCenterTypeId,
             string description,
             string joiningCode,
             string CreatorId,
             string imageUrl);

        void Edit(
            string id,
            string name,
            int addressId,
            int medicalCenterTypeId,
            string description,
            string joiningCode,
            string imageUrl);

        MedicalCenterQueryServiceModel All(
            string type = null,
            string country = null,
            string searchTerm = null,
            MedicalCentersSorting sorting = MedicalCentersSorting.DateCreated,
            int currentPage = 1,
            int medicalCentersPerPage = int.MaxValue);

        MedicalCenterServiceModel Details(string medicalCenterId);

        IEnumerable<MedicalCenterServiceModel> GetMedicalCenters();

        IEnumerable<MedicalCenterTypeServiceModel> GetMedicalCenterTypes();

        IEnumerable<MedicalCenterAddressServiceModel> GetAddresses();

        IEnumerable<string> AllCountries();

        IEnumerable<string> AllTypes();

        bool MedicalCenterAddressExists(int addressId);

        bool MedicalCenterTypeExists(int typeId);

        bool IsJoiningCodeUsed(string joiningCode);

        bool IsJoiningCodeCorrect(string joiningCode, string medicalCenterId);

        string GetJoiningCode(string medicalCenterId);

        bool IsCreator(string userId, string medicalCenterId);

        string GetMedicalCenterIdByUser(string userId);
    }
}
