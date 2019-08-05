namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MIS.Models;

    using Models;

    using ViewModels.View.Invitation;

    public interface IUserService
    {
        Task AddToCompanyAsync(Company company, string id);

        Task SetInvitationAsync(Invitation invitation, string id);

        Task<string> SetReceiptAsync(Receipt receipt, string username);

        Task<IEnumerable<MISUserServiceModel>> GetAllUsersAsync();
    }
}