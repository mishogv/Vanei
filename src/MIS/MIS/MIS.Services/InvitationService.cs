namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    using ViewModels.View.Invitation;

    public class InvitationService : IInvitationService
    {
        private readonly MISDbContext dbContext;

        public InvitationService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public async Task<InvitationServiceModel> AcceptInvitationAsync(int invitationId)
        {
            //TODO : IF CURRENT USER IS OWNER OF COMPANY WHAT HAPPENED
            //TODO : DROP HIS ROLE AND DELETE OLD COMPANY IF HAVE OR MAKE OTHER AS OWNER.
            
           var invitation = await this.dbContext.Invitations
                                      .Include(x => x.Company)
                                      .Include(x => x.User)
                                      .FirstOrDefaultAsync(x => x.Id == invitationId);

           invitation.Company.Employees.Add(invitation.User);

           this.dbContext.Update(invitation.Company);
           this.dbContext.Remove(invitation);
           await this.dbContext.SaveChangesAsync();

           return invitation.MapTo<InvitationServiceModel>();
        }

        public async Task<InvitationServiceModel> DeclineInvitationAsync(int invitationId)
        {
            var invitation =  await this.dbContext.Invitations.FirstOrDefaultAsync(x => x.Id == invitationId);

            this.dbContext.Remove(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }
    }
}