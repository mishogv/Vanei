namespace MIS.Services
{
    using System;
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

            if (user == null)
            {
                return;
            }
            
            company.Employees.Add(user);
        }

        public async Task SetInvitationAsync(Invitation invitation, string id)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return;
            }

            invitation.User = user;
        }

        public async Task<string> SetReceiptAsync(Receipt receipt, string username)
        {
            var user = await this.dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user?.CompanyId == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            receipt.User = user;

            return user.CompanyId;
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