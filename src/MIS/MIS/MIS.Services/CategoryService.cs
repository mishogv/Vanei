namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper.QueryableExtensions;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    using ViewModels.Input.Category;

    public class CategoryService : ICategoryService
    {
        private readonly IWareHouseService wareHouseService;
        private readonly MISDbContext dbContext;

        public CategoryService(IWareHouseService wareHouseService, MISDbContext dbContext)
        {
            this.wareHouseService = wareHouseService;
            this.dbContext = dbContext;
        }

        public async Task<CategoryServiceModel> CreateAsync(string name, string warehouseId)
        {
            var category = new Category()
            {
                Name = name
            };

            await this.wareHouseService.AddCategoryAsync(category, warehouseId);
            await this.dbContext.AddAsync(category);
            this.dbContext.Update(category.WareHouse);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> EditAsync(string id, string name)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return null;
            }

            category.Name = name;
            this.dbContext.Update(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> DeleteAsync(string id)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return null;
            }

            this.dbContext.Remove(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> GetCategoryAsync(string id)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return category?.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> SetCategoryAsync(Product product, string id)
        {
            var category = await this.dbContext.Categories
                                     .Include(x => x.WareHouse)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return null;
            }

            product.Category = category;
            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(string companyId)
        {
            var categories = await this.dbContext.Categories
                      .Include(x => x.WareHouse)
                      .Include(x => x.Products)
                      .Where(x => x.WareHouse.CompanyId == companyId)
                      .To<CategoryServiceModel>()
                      .ToListAsync();


            return categories;
        }


        public IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(string warehouseId)
        {
            return this.dbContext
                       .Categories
                       .Where(x => x.WareHouseId == warehouseId)
                       .ProjectTo<CreateCategoryWareHouseModel>()
                       .ToList();
        }
    }
}