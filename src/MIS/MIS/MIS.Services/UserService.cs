namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public class UserService : IUserService
    {
        private readonly MISDbContext dbContext;

        public UserService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //public async Task<int?> GetUserWarehouseIdAsync(string username)
        //{
        //    var user = await this.dbContext.Users
        //                    .Include(x => x.Company)
        //                    .ThenInclude(x => x.WareHouse)
        //                    .FirstOrDefaultAsync(x => x.UserName == username);
        //    var warehouseId = user.Company.WareHouseId;


        //   return warehouseId;
        //}
        public async Task<MISUser> GetUserByUsernameAsync(string username)
        {
            return await this.dbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}