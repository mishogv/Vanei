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

    using ViewModels.View.Product;

    public class ProductService : IProductService
    {
        private readonly MISDbContext db;
        private readonly ICategoryService categoryService;

        public ProductService(MISDbContext db, ICategoryService categoryService)
        {
            this.db = db;
            this.categoryService = categoryService;
        }

        public async Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, string categoryId, string warehouseId)
        {
            var product = new Product()
            {
                BarCode = barcode,
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            await this.categoryService.SetCategoryAsync(product, categoryId);

            product.WareHouse = product.Category.WareHouse;
            product.Category.Products.Add(product);
            await this.db.AddAsync(product);
            this.db.Update(product.Category);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> GetProductAsync(string id)
        {
            var product = await this.db.Products
                              .FirstOrDefaultAsync(x => x.Id == id);

            return product?.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> DeleteAsync(string id)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return null;
            }

            this.db.Remove(product);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> SetProductAsync(ReceiptProduct receiptProduct, string id)
        {
            var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return null;
            }

            receiptProduct.Product = product;

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> UpdateAsync(string id, string name, decimal price, double quantity, string barcode, string categoryId)
        {
            var product = await this.db.Products
                                    .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return null;
            }

            product.Name = name;
            product.Price = price;
            product.Quantity = quantity;

            await this.categoryService.SetCategoryAsync(product, categoryId);
            
            product.Category.Products.Add(product);
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
                                    .To<ShowReceiptProductViewModel>()
                                    .ToList();

            await Task.CompletedTask;

            return products;
        }
    }
}