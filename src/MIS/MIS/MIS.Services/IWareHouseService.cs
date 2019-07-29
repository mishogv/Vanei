namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    using ViewModels.Input.Category;

    public interface IWareHouseService
    {
        Task<WareHouseServiceModel> CreateAsync(string name, string companyId);

        Task<WareHouseServiceModel> GetWareHouseAsync(string id);

        Task<WareHouseServiceModel> DeleteAsync(string id);

        Task<WareHouseServiceModel> EditAsync(string id, string name);

        Task<WareHouseServiceModel> MakeFavoriteAsync(string id, string companyId);

        Task<bool> AddCategoryAsync(Category category, string warehouseId);

        Task<IEnumerable<WareHouseServiceModel>> GetWarehousesByCompanyIdAsync(string companyId);
    }
}