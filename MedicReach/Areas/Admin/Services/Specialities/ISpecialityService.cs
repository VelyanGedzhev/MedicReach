namespace MedicReach.Areas.Admin.Services.Specialities
{
    public interface ISpecialityService
    {
        void Add(string name);

        bool IsExisting(string name);
    }
}
