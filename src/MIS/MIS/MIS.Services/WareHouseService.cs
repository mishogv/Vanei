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

    using ViewModels.Input.Category;

    public class WareHouseService : IWareHouseService
    {
        private readonly MISDbContext dbContext;

        public WareHouseService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<WareHouseServiceModel> CreateAsync(string name, string userId)
        {
            var user = await this.dbContext.Users
                                 .Include(x => x.Company)
                                 .ThenInclude(x => x.WareHouses)
                                 .FirstOrDefaultAsync(x => x.Id == userId);

            if (user?.CompanyId == null)
            {
                return null;
            }

            var warehouse = new WareHouse
            {
                Name = name,
                Company = user.Company,
                IsFavorite = false
            };

            if (!user.Company.WareHouses.Any())
            {
                warehouse.IsFavorite = true;
            }

            this.dbContext.Add(warehouse);
            await this.dbContext.SaveChangesAsync();

            this.dbContext.Update(user.Company);

            await this.dbContext.SaveChangesAsync();

            var result = warehouse.MapTo<WareHouseServiceModel>();

            return result;
        }

        public async Task<WareHouseServiceModel> GetWareHouseByUserNameAsync(string username)
        {
            var user = await this.dbContext.Users
                           .Include(x => x.Company)
                           .ThenInclude(x => x.WareHouses)
                           .FirstOrDefaultAsync(x => x.UserName == username);

            var warehouse = user.Company.WareHouses.FirstOrDefault(x => x.IsFavorite);

            return warehouse.MapTo<WareHouseServiceModel>();
        }

        public async Task<WareHouseServiceModel> GetWareHouseByNameAsync(string name)
        {
           var wareHouse =  await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Name == name);
           var result = wareHouse?.MapTo<WareHouseServiceModel>();

           return result;
        }

        public IEnumerable<CreateCategoryWareHouseModel> GetAllUserWareHousesByUserName(string username)
        {
            var result = this.dbContext.Users
                                 .Include(x => x.Company)
                                 .ThenInclude(x => x.WareHouses)
                                 .FirstOrDefaultAsync(x => x.UserName == username).GetAwaiter().GetResult()
                                 .Company
                                 .WareHouses
                                 .OrderByDescending(x => x.IsFavorite)
                                 .Select(x => x.MapTo<CreateCategoryWareHouseModel>())
                                 .ToList();

            return result;
        }

        public async Task<IEnumerable<string>> GetAllCategoriesNamesAsync(string wareHouseName, string username)
        {
            var user = await this.dbContext.Users
                                      .Include(x => x.Company)
                                      .ThenInclude(x => x.WareHouses)
                                      .ThenInclude(x => x.Categories)
                                      .FirstOrDefaultAsync(x => x.UserName == username);

            var warehouse = user.Company.WareHouses.FirstOrDefault(x => x.Name == wareHouseName);

            var categories = warehouse?.Categories.Select(x => x.Name);

            return categories;
        }

        public IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int id)
        {
            var warehouse = this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.CompanyId == id).GetAwaiter().GetResult();

            return warehouse?.Products.AsQueryable().Select(x => x.MapTo<ProductServiceModel>()).AsQueryable();
        }
    }
}