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
        private readonly IWareHouseService wareHouseService;
        private readonly MISDbContext dbContext;

        public CategoryService(IWareHouseService wareHouseService, MISDbContext dbContext)
        {
            this.wareHouseService = wareHouseService;
            this.dbContext = dbContext;
        }

        public async Task<CategoryServiceModel> CreateAsync(string name, string wareHouseName)
        {
            var wareHouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Name == wareHouseName);

            var category = new Category()
            {
                Name = name,
                WareHouse = wareHouse
            };

            this.dbContext.Add(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }
    }
}