namespace MIS.Services
{
    using System.Threading.Tasks;

    using ServicesModels;

    public interface IProductService
    {
        Task<ProductServiceModel> Create(string name, decimal price, double quantity, string barcode);
    }
}