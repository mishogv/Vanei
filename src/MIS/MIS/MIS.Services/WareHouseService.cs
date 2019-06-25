namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using AutoMapper.QueryableExtensions;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    public class WareHouseService : IWareHouseService
    {
        private readonly MISDbContext dbContext;

        public WareHouseService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<WareHouseServiceModel> CreateAsync(string name, string userId)
        {
            var user = await this.dbContext.Users.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == userId);

            if (user?.CompanyId == null)
            {
                return null;
            }

            var warehouse = new WareHouse
            {
                Name = name,
                CompanyId = (int)user.CompanyId,
            };

            if (!user.Company.WareHouses.Any())
            {
                warehouse.IsFavorite = true;
            }

            this.dbContext.Add(warehouse);
            await this.dbContext.SaveChangesAsync();

            //user.Company.WareHouseId = warehouse.Id;
            this.dbContext.Update(user.Company);

            await this.dbContext.SaveChangesAsync();

            var result = warehouse.MapTo<WareHouseServiceModel>();

            return result;
        }

        public IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int? id)
        {
            var warehouse = this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.CompanyId == id).GetAwaiter().GetResult();

            return warehouse?.Products.AsQueryable().Select(x => x.MapTo<ProductServiceModel>()).AsQueryable();
        }
    }
}