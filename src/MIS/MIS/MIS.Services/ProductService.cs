namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    public class ProductService : IProductService
    {
        private readonly MISDbContext db;

        public ProductService(MISDbContext db)
        {
            this.db = db;
        }

        public async Task<ProductServiceModel> CreateAsync(string name, decimal price, double quantity, string barcode, string categoryName, string wareHouseName, string username)
        {
            var user = await this.db.Users
                                 .Include(x => x.Company)
                                 .ThenInclude(x => x.WareHouses)
                                 .ThenInclude(x => x.Categories)
                                 .FirstOrDefaultAsync(x => x.UserName == username);

            var warehouse = user.Company.WareHouses.FirstOrDefault(x => x.Name == wareHouseName);
            var category = warehouse?.Categories.FirstOrDefault(x => x.Name == categoryName);

            var product = new Product
            {
                Name = name,
                BarCode = barcode,
                Price = price,
                Quantity = quantity,
                WareHouse = warehouse,
                Category = category
            };

            await this.db.AddAsync(product);
            await this.db.SaveChangesAsync();

            return product.MapTo<ProductServiceModel>();
        }

        public async Task<IEnumerable<string>> GetAllProductsNamesByUsernameAsync(string username)
        {
            var user = this.db.Users
                                    .Include(x => x.Company)
                                    .ThenInclude(x => x.WareHouses)
                                    .ThenInclude(x => x.Products)
                                    .Where(x => x.UserName == username)
                                    .Take(1)
                                    .SelectMany(x => x.Company.WareHouses)
                                    .SelectMany(x => x.Products)
                                    .Select(x => x.Name)
                                    .ToList();

            //var names = user.Company.WareHouses.SelectMany(x => x.Products).Select(x => x.Name);

            return user;
        }
    }
}