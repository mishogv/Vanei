namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    using ViewModels.Input.Category;

    public interface IWareHouseService
    {
        Task<WareHouseServiceModel> CreateAsync(string name, int? companyId);

        Task<WareHouseServiceModel> GetWareHouseAsync(int id);

        Task<WareHouseServiceModel> DeleteAsync(int id);

        Task<WareHouseServiceModel> EditAsync(int id, string name);

        Task<WareHouseServiceModel> MakeFavoriteAsync(int id, int? companyId);

        Task<WareHouseServiceModel> GetWareHouseByUserNameAsync(string username);

        IEnumerable<WareHouseServiceModel> GetWarehousesByCompanyId(int? companyId);

        Task<WareHouseServiceModel> GetWareHouseByNameAsync(string name);

        IEnumerable<CreateCategoryWareHouseModel> GetAllUserWareHousesByUserName(string username);

        IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(int warehouseId);

        Task<IEnumerable<string>> GetAllCategoriesNamesAsync(string wareHouseName, string username);

        IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int id);
    }
}