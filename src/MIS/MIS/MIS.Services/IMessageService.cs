namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IMessageService
    {
        Task<MessageServiceModel> CreateAsync(string companyId, string username, string text, bool isNotification);

        Task<IEnumerable<MessageServiceModel>> GetAllAsync(string companyId);
    }
}