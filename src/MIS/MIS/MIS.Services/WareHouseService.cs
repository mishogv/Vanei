namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using Data;

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


            this.dbContext.Add(warehouse);
            await this.dbContext.SaveChangesAsync();

            //user.Company.WareHouseId = warehouse.Id;
            this.dbContext.Update(user.Company);

            await this.dbContext.SaveChangesAsync();

            var result = new WareHouseServiceModel()
            {
                Name = warehouse.Name,
                CompanyId = warehouse.CompanyId,
            };

            return result;
        }

        public IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int? id)
        {
            var warehouse = this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.CompanyId == id).GetAwaiter().GetResult();

            return warehouse?.Products.Select(x => new ProductServiceModel()
            {
                Name = x.Name,
                Price = x.Price,
                BarCode = x.BarCode,
                WareHouseId = x.WareHouseId,
                CategoryId = x.CategoryId,
                Quantity = x.Quantity
            }).AsQueryable();
        }
    }
}