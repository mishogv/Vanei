namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    using ViewModels.Input.Category;

    public interface ICategoryService
    {
        Task<CategoryServiceModel> CreateAsync(string name, string warehouseId);

        Task<CategoryServiceModel> EditAsync(string id, string name);

        Task<CategoryServiceModel> DeleteAsync(string id);

        Task<CategoryServiceModel> GetCategoryAsync(string id);

        Task<CategoryServiceModel> SetCategoryAsync(Product product, string id);

        Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(string companyId);

        IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(string warehouseId);
    }
}