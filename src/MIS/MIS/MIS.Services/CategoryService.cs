namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

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
    }
}