namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;
    using Common.Extensions;

    using Data;

    using Mapping;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class InvitationService : IInvitationService
    {
        private readonly MISDbContext dbContext;
        private readonly UserManager<MISUser> userManager;
        private readonly ICompanyService companyService;
        private readonly IUserService userService;

        public InvitationService(MISDbContext dbContext,
            UserManager<MISUser> userManager,
            ICompanyService companyService,
            IUserService userService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.companyService = companyService;
            this.userService = userService;
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
            var invitation = new Invitation();

            await this.companyService.SetCompanyAsync(invitation, (int)companyId);
            await this.userService.SetInvitationAsync(invitation, userId);

            await this.dbContext.AddAsync(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
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

            invitation.ThrowIfNull(nameof(invitation));

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