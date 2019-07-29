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
        Task<WareHouseServiceModel> CreateAsync(string name, int? companyId);

        Task<WareHouseServiceModel> GetWareHouseAsync(int id);

        Task<WareHouseServiceModel> DeleteAsync(int id);

        Task<WareHouseServiceModel> EditAsync(int id, string name);

        Task<WareHouseServiceModel> MakeFavoriteAsync(int id, int? companyId);

        Task<bool> AddCategoryAsync(Category category, int warehouseId);

        Task<IEnumerable<WareHouseServiceModel>> GetWarehousesByCompanyIdAsync(int? companyId);
    }
}