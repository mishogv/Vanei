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

    public class CategoryService : ICategoryService
    {
        private readonly MISDbContext dbContext;

        public CategoryService(IWareHouseService wareHouseService, MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CategoryServiceModel> CreateAsync(string name, int warehouseId)
        {
            var currentWarehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == warehouseId);
            var category = new Category()
            {
                Name = name
            };

            currentWarehouse.Categories.Add(category);

            this.dbContext.Update(currentWarehouse);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> EditAsync(int id, string name)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            category.Name = name;
            this.dbContext.Update(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> DeleteAsync(int id)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            this.dbContext.Remove(category);
           await  this.dbContext.SaveChangesAsync();

           return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> GetCategoryAsync(int id)
        {
            // TODO : if null
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(int companyId)
        {
            var categories = await this.dbContext.WareHouses
                                 .Include(x => x.Categories)
                                 .ThenInclude(x => x.Products)
                                 .Where(x => x.CompanyId == companyId)
                                 .SelectMany(x => x.Categories)
                                 .To<CategoryServiceModel>()
                                 .ToListAsync();
            

            return categories;
        }
    }
}