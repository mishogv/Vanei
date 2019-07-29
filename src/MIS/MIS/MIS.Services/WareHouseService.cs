﻿namespace MIS.Services
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

    public class WareHouseService : IWareHouseService
    {
        private readonly MISDbContext dbContext;

        public WareHouseService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<WareHouseServiceModel> CreateAsync(string name, int? companyId)
        {
            var company = await this.dbContext
                                    .Companies
                                    .Include(x => x.WareHouses)
                                    .FirstOrDefaultAsync(x => x.Id == companyId);

            var warehouse = new WareHouse()
            {
                Name = name,
            };

            if (company.WareHouses.Count(x => x.IsFavorite) == 0)
            {
                warehouse.IsFavorite = true;
            }

            company.WareHouses.Add(warehouse);

            this.dbContext.Update(company);
            await this.dbContext.SaveChangesAsync();

            var result = warehouse.MapTo<WareHouseServiceModel>();

            return result;
        }

        public async Task<WareHouseServiceModel> GetWareHouseAsync(int id)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);

            return warehouse.MapTo<WareHouseServiceModel>();
        }

        public async Task<WareHouseServiceModel> DeleteAsync(int id)
        {
            var warehouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Id == id);
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
            warehouse.ThrowIfNull(nameof(warehouse));
            warehouse.Categories.Add(category);
            var result = await this.dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<WareHouseServiceModel> GetWareHouseByUserNameAsync(string username)
        {
            var user = await this.dbContext.Users
                           .Include(x => x.Company)
                           .ThenInclude(x => x.WareHouses)
                           .FirstOrDefaultAsync(x => x.UserName == username);

            var warehouse = user.Company.WareHouses.FirstOrDefault(x => x.IsFavorite);

            return warehouse.MapTo<WareHouseServiceModel>();
        }

        public IEnumerable<WareHouseServiceModel> GetWarehousesByCompanyId(int? companyId)
        {
            var wareHouses = this.dbContext
                              .Companies
                              .Include(x => x.WareHouses)
                              .ThenInclude(x => x.Products)
                              .Where(x => x.Id == companyId)
                              .SelectMany(x => x.WareHouses)
                              .To<WareHouseServiceModel>()
                              .ToList();

            return wareHouses;
        }

        public async Task<WareHouseServiceModel> GetWareHouseByNameAsync(string name)
        {
            var wareHouse = await this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.Name == name);
            var result = wareHouse?.MapTo<WareHouseServiceModel>();

            return result;
        }

        public IEnumerable<CreateCategoryWareHouseModel> GetAllUserWareHousesByUserName(string username)
        {
            var result = this.dbContext.Users
                                 .Include(x => x.Company)
                                 .ThenInclude(x => x.WareHouses)
                                 .ThenInclude(x => x.Products)
                                 .FirstOrDefaultAsync(x => x.UserName == username).GetAwaiter().GetResult()
                                 .Company
                                 .WareHouses
                                 .OrderByDescending(x => x.IsFavorite)
                                 .Select(x => x.MapTo<CreateCategoryWareHouseModel>())
                                 .ToList();

            return result;
        }

        public IEnumerable<CreateCategoryWareHouseModel> GetAllCategories(int warehouseId)
        {
            return this.dbContext
                       .Categories
                       .Where(x => x.WareHouseId == warehouseId)
                       .ProjectTo<CreateCategoryWareHouseModel>()
                       .ToList();
        }

        public Task<IEnumerable<string>> GetAllCategoriesNamesAsync(string wareHouseName, string username)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ProductServiceModel> GetAllProductsByWarehouseId(int id)
        {
            var warehouse = this.dbContext.WareHouses.FirstOrDefaultAsync(x => x.CompanyId == id).GetAwaiter().GetResult();

            return warehouse?.Products.AsQueryable().Select(x => x.MapTo<ProductServiceModel>()).AsQueryable();
        }
    }
}