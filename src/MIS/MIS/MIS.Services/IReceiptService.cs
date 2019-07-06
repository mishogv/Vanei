namespace MIS.Services
{
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    public interface IReceiptService
    {
        Task<ReceiptServiceModel> GetCurrentOpenedReceiptByUsernameAsync(string username);

        Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username);
    }
}