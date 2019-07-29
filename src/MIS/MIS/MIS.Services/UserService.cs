namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using ViewModels.View.Invitation;

    public class UserService : IUserService
    {
        private readonly MISDbContext dbContext;

        public UserService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddToCompanyAsync(Company company, string id)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            company.Employees.Add(user);
        }

        public async Task SetInvitationAsync(Invitation invitation, string id)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            invitation.User = user;
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

    }
}