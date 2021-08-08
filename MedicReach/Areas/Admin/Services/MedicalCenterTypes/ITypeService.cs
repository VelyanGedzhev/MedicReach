namespace MedicReach.Areas.Admin.Services.MedicalCenterTypes
{
    public interface ITypeService
    {
        void Add(string name);

        bool IsExisting(string name);
    }
}
