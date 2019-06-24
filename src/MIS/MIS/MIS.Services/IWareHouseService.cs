namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface IWareHouseService
    {
        Task<WareHouseServiceModel> CreateAsync(string name, string userId);

        IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int? id);
    }
}