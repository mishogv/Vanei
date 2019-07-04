namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    using ViewModels.Input.Category;

    public interface IWareHouseService
    {
        Task<WareHouseServiceModel> CreateAsync(string name, string userId);

        Task<WareHouseServiceModel> GetWareHouseByUserNameAsync(string username);

        Task<WareHouseServiceModel> GetWareHouseByNameAsync(string name);

        IEnumerable<CreateCategoryWareHouseModel> GetAllUserWareHousesByUserName(string username);

        Task<IEnumerable<string>> GetAllCategoriesNamesAsync(string wareHouseName, string username);

        IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int id);
    }
}