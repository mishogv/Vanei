namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper.QueryableExtensions;

    using Common.Extensions;

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

        public async Task<CategoryServiceModel> CreateAsync(string name, int warehouseId)
        {
            var category = new Category()
            {
                Name = name
            };

            await this.wareHouseService.AddCategoryAsync(category, warehouseId);

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> EditAsync(int id, string name)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            category.ThrowIfNull(nameof(category));

            category.Name = name;
            this.dbContext.Update(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> DeleteAsync(int id)
        {
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            category.ThrowIfNull(nameof(category));

            this.dbContext.Remove(category);
            await this.dbContext.SaveChangesAsync();

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> GetCategoryAsync(int id)
        {
            // TODO : if null
            var category = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            category.ThrowIfNull(nameof(category));

            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> SetCategoryAsync(Product product, int id)
        {
            var category = await this.dbContext.Categories.Include(x => x.WareHouse).FirstOrDefaultAsync(x => x.Id == id);

            category.ThrowIfNull(nameof(category));
            product.Category = category;
            return category.MapTo<CategoryServiceModel>();
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllByCompanyIdAsync(int companyId)
        {
            var categories = await this.dbContext.Categories
                      .Include(x => x.WareHouse)
                      .Include(x => x.Products)
                      .Where(x => x.WareHouse.CompanyId == companyId)
                      .To<CategoryServiceModel>()
                      .ToListAsync();


            return categories;
        }


        public IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(int warehouseId)
        {
            return this.dbContext
                       .Categories
                       .Where(x => x.WareHouseId == warehouseId)
                       .ProjectTo<CreateCategoryWareHouseModel>()
                       .ToList();
        }
    }
}