namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper.QueryableExtensions;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    using ViewModels.View.Product;

    public class ProductService : IProductService
    {
        private readonly MISDbContext db;

        public ProductService(MISDbContext db)
        {
            this.db = db;
        }

        public async Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, int categoryId, int warehouseId)
        {
            var product = new Product()
            {
                BarCode = barcode,
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            var category = await this.db.Categories
                                     .Include(x => x.WareHouse)
                                     .FirstOrDefaultAsync(x => x.Id == categoryId);

            product.WareHouse = category.WareHouse;
            category.Products.Add(product);
            await this.db.AddAsync(product);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> GetProductAsync(int id)
        {
            var product = await this.db.Products
                              .FirstOrDefaultAsync(x => x.Id == id);

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> DeleteAsync(int id)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == id);

            this.db.Remove(product);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> UpdateAsync(int id, string name, decimal price, double quantity, string barcode, int categoryId)
        {

            var product = await this.db.Products
                                    .FirstOrDefaultAsync(x => x.Id == id);

            var category = await this.db.Categories
                                    .FirstOrDefaultAsync(x => x.Id == categoryId);

            product.Name = name;
            product.Price = price;
            product.Quantity = quantity;
            product.Category = category;

            this.db.Update(product);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<IEnumerable<ShowReceiptProductViewModel>> GetAllProductsByUsernameAsync(string username)
        {
            //TODO : Refactor
            var products = this.db.Users
                                    .Include(x => x.Company)
                                    .ThenInclude(x => x.WareHouses)
                                    .ThenInclude(x => x.Products)
                                    .Where(x => x.UserName == username)
                                    .Take(1)
                                    .SelectMany(x => x.Company.WareHouses)
                                    .SelectMany(x => x.Products)
                                    .ProjectTo<ShowReceiptProductViewModel>()
                                    .ToList();

            await Task.CompletedTask;

            return products;
        }
    }
}