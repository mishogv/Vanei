namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ServicesModels;

    using ViewModels.View.Invitation;

    public interface IInvitationService
    {
        Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id);

        Task<InvitationServiceModel> InviteAsync(int? companyId, string userId);

        Task<IEnumerable<InvitationUserViewModel>> GetAllUsersAsync();

        Task<InvitationServiceModel> AcceptInvitationAsync(int invitationId, bool isOwner);

        Task<InvitationServiceModel> DeclineInvitationAsync(int invitationId);
    }
}