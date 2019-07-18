namespace MIS.Services
{
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    public interface IReceiptService
    {
        Task<ReceiptServiceModel> GetCurrentOpenedReceiptByUsernameAsync(string username);

        Task<ReceiptProductServiceModel> AddProductToOpenedReceiptByUsernameAsync(string username, int id, double quantity);

        Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username);

        Task<ReceiptServiceModel> DeleteReceiptAsync(string username);
    }
}