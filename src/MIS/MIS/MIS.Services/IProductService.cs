namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    using ViewModels.View.Product;

    public interface IProductService
    {
        Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, string categoryName, string wareHouseName, string username);

        Task<IEnumerable<ShowReceiptProductViewModel>> GetAllProductsByUsernameAsync(string username);
    }
}