namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Mapping;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    using ViewModels.View.Invitation;

    public class InvitationService : IInvitationService
    {
        private readonly MISDbContext dbContext;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;

        public InvitationService(MISDbContext dbContext,
            UserManager<MISUser> userManager,
            ICompanyService companyService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.companyService = companyService;
        }

        public async Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id)
        {
            var invitations = await this.dbContext.Invitations
                                        .Where(x => x.UserId == id)
                                        .To<InvitationServiceModel>()
                                        .ToListAsync();

            return invitations;
        }

        public async Task<InvitationServiceModel> InviteAsync(int? companyId, string userId)
        {
            var company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            //TODO : IF NULL

            var invitation = new Invitation
            {
                Company = company,
                User = user
            };

            await this.dbContext.AddAsync(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }

        public async Task<IEnumerable<InvitationUserViewModel>> GetAllUsersAsync()
        {
            var users = await this.dbContext.Users
                       .Include(x => x.Company)
                       .Include(x => x.Invitations)
                       .To<InvitationUserViewModel>()
                       .ToListAsync();

            return users;
        }

        public async Task<InvitationServiceModel> AcceptInvitationAsync(int invitationId, bool isOwner)
        {
            //TODO : IF CURRENT USER IS OWNER OF COMPANY WHAT HAPPENED
            //TODO : DROP HIS ROLE AND DELETE OLD COMPANY IF HAVE OR MAKE OTHER AS OWNER.

            var invitation = await this.dbContext.Invitations
                                       .Include(x => x.Company)
                                       .Include(x => x.User)
                                       .ThenInclude(x => x.Company)
                                       .FirstOrDefaultAsync(x => x.Id == invitationId);

            var user = invitation.User;

            if (isOwner)
            {
                await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.CompanyOwnerRole);

                if (user.Company.Employees.Count == 1)
                {
                    await this.companyService.DeleteAsync(user.Company.Id);
                }
                else
                {
                   var newOwner =  user.Company.Employees.FirstOrDefault(x => x.Id != user.Id);
                   await this.userManager.AddToRoleAsync(newOwner, GlobalConstants.CompanyOwnerRole);
                }
            }

            user.Company = invitation.Company;

            this.dbContext.Update(invitation.User);
            this.dbContext.Remove(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }

        public async Task<InvitationServiceModel> DeclineInvitationAsync(int invitationId)
        {
            var invitation = await this.dbContext.Invitations.FirstOrDefaultAsync(x => x.Id == invitationId);

            this.dbContext.Remove(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }
    }
}