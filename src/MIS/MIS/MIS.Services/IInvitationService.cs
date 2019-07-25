namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    public interface IInvitationService
    {
        Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id);
    }
}