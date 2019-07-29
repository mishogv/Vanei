namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    public interface IReceiptService
    {
        Task<ReceiptServiceModel> GetCurrentOpenedReceiptByUsernameAsync(string username);

        Task<ReceiptProductServiceModel> AddProductToOpenedReceiptByUsernameAsync(string username, string id, double quantity);

        Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username);

        Task<ReceiptServiceModel> DeleteReceiptAsync(string username);

        Task<ReceiptServiceModel> GetReceiptAsync(int id);

        Task<ReceiptServiceModel> DeleteReceiptByIdAsync(int id);

        Task<IEnumerable<ReceiptServiceModel>> SetReceiptsAsync(Report report, DateTime from, DateTime to, string companyId);
    }
}