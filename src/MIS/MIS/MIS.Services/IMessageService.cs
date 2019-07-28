namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IMessageService
    {
        Task<MessageServiceModel> CreateAsync(int companyId, string username, string text, bool isNotification);

        Task<IEnumerable<MessageServiceModel>> GetAll(int companyId);
    }
}