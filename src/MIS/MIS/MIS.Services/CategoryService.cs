namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

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

        public async Task<CategoryServiceModel> CreateAsync(string name, int warehouseId)
        {
            //this.wareHouseService.GetWareHouseByIdAsync(warehouseId);
            var category = new Category()
            {
                Name = name,
                WareHouseId = warehouseId
            };

            this.dbContext.Add(category);
            await this.dbContext.SaveChangesAsync();

            

            return new CategoryServiceModel()
            {
                Name = category.Name,
                WareHouseId = category.WareHouseId,
                WareHouse = category.WareHouse,
                Products = category.Products
            };
        }
    }
}