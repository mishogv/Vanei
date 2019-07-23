namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    using ViewModels.View.Product;

    public interface IProductService
    {
        Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, int categoryId, int warehouseId);

        Task<IEnumerable<ShowReceiptProductViewModel>> GetAllProductsByUsernameAsync(string username);
    }
}