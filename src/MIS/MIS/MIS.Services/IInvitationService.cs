namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IInvitationService
    {
        Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id);

        Task<InvitationServiceModel> InviteAsync(string companyId, string userId);

        Task<InvitationServiceModel> AcceptInvitationAsync(string invitationId, bool isOwner);

        Task<InvitationServiceModel> DeclineInvitationAsync(string invitationId);
    }
}