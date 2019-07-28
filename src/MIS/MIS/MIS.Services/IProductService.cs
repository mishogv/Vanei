namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    using ViewModels.View.Product;

    public interface IProductService
    {
        Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, int categoryId, int warehouseId);

        Task<ProductServiceModel> GetProductAsync(int id);

        Task<ProductServiceModel> DeleteAsync(int id);

        Task<ProductServiceModel> UpdateAsync(int id, string name, decimal price, double quantity, string barcode, int categoryId);

        Task<IEnumerable<ShowReceiptProductViewModel>> GetAllProductsByUsernameAsync(string username);
    }
}