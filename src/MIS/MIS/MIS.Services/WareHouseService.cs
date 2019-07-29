namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class WareHouseService : IWareHouseService
    {
        private readonly MISDbContext dbContext;
        private readonly ICompanyService companyService;

        public WareHouseService(MISDbContext dbContext, ICompanyService companyService)
        {
            this.dbContext = dbContext;
            this.companyService = companyService;
        }

        public async Task<WareHouseServiceModel> CreateAsync(string name, int? companyId)
        {
            var warehouse = new WareHouse()
            {
                Name = name,
            };

            await this.companyService.SetCompanyAsync(warehouse, (int)companyId);

            this.dbContext.Update(warehouse.Company);
            await this.dbContext.SaveChangesAsync();

            var result = warehouse.MapTo<WareHouseServiceModel>();

            return result;
        }

        public async Task<WareHouseServiceModel> GetWareHouseAsync(int id)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);

            return warehouse?.MapTo<WareHouseServiceModel>();
        }

        public async Task<WareHouseServiceModel> DeleteAsync(int id)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);

            if (warehouse == null)
            {
                return null;
            }

            var newWarehouse = new WareHouse();
            this.dbContext.Remove(warehouse);

            if (warehouse.IsFavorite)
            {
                newWarehouse = await this.dbContext.WareHouses.Where(x => x.CompanyId == warehouse.CompanyId)
                     .FirstOrDefaultAsync(x => x.Id != id);

                if (newWarehouse != null)
                {
                    newWarehouse.IsFavorite = true;
                    this.dbContext.Update(newWarehouse);
                }
            }

            await this.dbContext.SaveChangesAsync();

            return warehouse.MapTo<WareHouseServiceModel>();
        }

        public async Task<WareHouseServiceModel> EditAsync(int id, string name)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);

            if (warehouse == null)
            {
                return null;
            }

            warehouse.Name = name;

            this.dbContext.Update(warehouse);
            await this.dbContext.SaveChangesAsync();

            return warehouse.MapTo<WareHouseServiceModel>();
        }

        public async Task<WareHouseServiceModel> MakeFavoriteAsync(int id, int? companyId)
        {
            var warehouseFromDb = await this.dbContext.Companies
                                 .Include(x => x.WareHouses)
                                 .Where(x => x.Id == companyId)
                                 .SelectMany(x => x.WareHouses)
                                 .Where(x => x.IsFavorite)
                                 .FirstOrDefaultAsync();

            warehouseFromDb.IsFavorite = false;

            var currentWarehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);
            currentWarehouse.IsFavorite = true;
            this.dbContext.Update(warehouseFromDb);
            this.dbContext.Update(currentWarehouse);

            await this.dbContext.SaveChangesAsync();

            return currentWarehouse.MapTo<WareHouseServiceModel>();
        }

        public async Task<bool> AddCategoryAsync(Category category, int warehouseId)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == warehouseId);

            if (warehouse == null)
            {
                return false;
            }

            warehouse.Categories.Add(category);
            var result = await this.dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<WareHouseServiceModel>> GetWarehousesByCompanyIdAsync(int? companyId)
        {
            var warehouses = await this.dbContext
                                 .WareHouses
                                 .Include(x => x.Products)
                                 .Where(x => x.CompanyId == companyId)
                                 .To<WareHouseServiceModel>()
                                 .ToListAsync();

            return warehouses;
        }
    }
}