namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

    using Mapping;

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
            var wareHouseServiceModel = await this.wareHouseService.GetWareHouseByNameAsync(wareHouseName);
            var wareHouse = wareHouseServiceModel.MapTo<WareHouse>();

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