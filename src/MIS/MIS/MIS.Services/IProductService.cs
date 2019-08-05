namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    using ViewModels.View.Product;

    public interface IProductService
    {
        Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, string categoryId, string warehouseId);

        Task<ProductServiceModel> GetProductAsync(string id);

        Task<ProductServiceModel> DeleteAsync(string id);

        Task<ProductServiceModel> SetProductAsync(ReceiptProduct product, string id);

        Task<ProductServiceModel> UpdateAsync(string id, string name, decimal price, double quantity, string barcode, string categoryId);

        Task<IEnumerable<ProductShowReceiptViewModel>> GetAllProductsCompanyIdAsync(string companyId);
    }
}