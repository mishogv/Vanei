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

        public async Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity,
            string barcode, string categoryId, string warehouseId)
        {
            var product = new Product()
            {
                BarCode = barcode,
                Name = name,
                Price = price,
                Quantity = quantity,
            };

            await this.categoryService.SetCategoryAsync(product, categoryId);

            if (product.Category == null)
            {
                return null;
            }

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
                                    .Include(x => x.ReceiptProducts)
                                    .ThenInclude(x => x.Receipt)
                                    .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return null;
            }

            product.Name = name;
            product.Price = price;
            product.Quantity = quantity;

            foreach (var productReceiptProduct in product.ReceiptProducts)
            {
                productReceiptProduct.Receipt.Total -= productReceiptProduct.Total;
                productReceiptProduct.Total = product.Price * (decimal) productReceiptProduct.Quantity;
                productReceiptProduct.Receipt.Total += productReceiptProduct.Total;
            }


            await this.categoryService.SetCategoryAsync(product, categoryId);

            product.Category.Products.Add(product);
            this.db.Update(product);
            this.db.UpdateRange(product.ReceiptProducts);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<IEnumerable<ShowReceiptProductViewModel>> GetAllProductsCompanyIdAsync(string companyId)
        {
            var products = await this.db.Products
                                     .Include(x => x.WareHouse)
                                     .Where(x => x.WareHouse.CompanyId == companyId)
                                     .To<ShowReceiptProductViewModel>()
                                     .ToListAsync();

            return products;
        }
    }
}