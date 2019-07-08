namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface IProductService
    {
        Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, string categoryName, string wareHouseName, string username);

        Task<IEnumerable<string>> GetAllProductsNamesByUsernameAsync(string username);
    }
}