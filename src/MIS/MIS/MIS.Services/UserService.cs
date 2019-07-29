namespace MIS.Services
{
    using System.Threading.Tasks;

    using Common.Extensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

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

            user.ThrowIfNull(nameof(user));

            company.Employees.Add(user);
        }
    }
}