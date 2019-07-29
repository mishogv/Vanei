namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    using ViewModels.Input.Category;

    public interface ICategoryService
    {
        Task<CategoryServiceModel> CreateAsync(string name, int warehouseId);

        Task<CategoryServiceModel> EditAsync(int id, string name);

        Task<CategoryServiceModel> DeleteAsync(int id);

        Task<CategoryServiceModel> GetCategoryAsync(int id);

        Task<CategoryServiceModel> SetCategoryAsync(Product product, int id);

        Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(int companyId);


        IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(int warehouseId);
    }
}