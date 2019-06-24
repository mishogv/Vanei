namespace MIS.Services
{
    using System.Linq;

    using Data;

    using Microsoft.EntityFrameworkCore;

    public class NavigationMenuHelperService : INavigationMenuHelperService
    {
        private readonly MISDbContext dbContext;

        public NavigationMenuHelperService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool IsCompanyUserHaveWarehouse(string username)
        {
            var user = this.dbContext.Users
                           .Include(x => x.Company)
                           .ThenInclude(x => x.WareHouse)
                           .FirstOrDefault(x => x.UserName == username);

            return user?.Company != null;
        }
    }
}