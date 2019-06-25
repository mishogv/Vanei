namespace MIS.Services
{
    using System.Threading.Tasks;

    using ServicesModels;

    public interface ICategoryService
    {
        Task<CategoryServiceModel> CreateAsync(string name, int warehouseId);
    }
}