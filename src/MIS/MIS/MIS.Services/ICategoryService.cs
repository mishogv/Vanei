namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface ICategoryService
    {
        Task<CategoryServiceModel> CreateAsync(string name, int warehouseId);

        Task<CategoryServiceModel> EditAsync(int id, string name);

        Task<CategoryServiceModel> DeleteAsync(int id);

        Task<CategoryServiceModel> GetCategoryAsync(int id);

        Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(int companyId);
    }
}