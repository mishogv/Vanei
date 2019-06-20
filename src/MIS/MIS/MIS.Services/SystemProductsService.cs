namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

    public class SystemProductsService : ISystemProductsService
    {
        private readonly MISDbContext dbContext;

        public SystemProductsService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SystemProductServiceModel> CreateSystemProductAsync(string name, decimal price, string imgUrl, string description, string userId)
        {
            var systemProduct = new SystemProduct()
            {
                Name = name,
                Price = price,
                ImgUrl = imgUrl,
                Description = description,
                UserId = userId
            };

            await this.dbContext.SystemProducts.AddAsync(systemProduct);
            await this.dbContext.SaveChangesAsync();

            var product = new SystemProductServiceModel()
            {
                Id = systemProduct.Id,
                Name = systemProduct.Name,
                Price = systemProduct.Price,
                ImgUrl = systemProduct.ImgUrl,
                Description = systemProduct.Description,
                UserId = systemProduct.UserId
            };

            return product;
        }

        public async Task<SystemProductServiceModel> GetSystemProductByIdAsync(int id)
        {
            var productFromDb = await this.dbContext.SystemProducts.FindAsync(id);

            if (productFromDb == null)
            {
                return null;
            }

            var product = new SystemProductServiceModel()
            {
                Id = productFromDb.Id,
                Name = productFromDb.Name,
                Price = productFromDb.Price,
                ImgUrl = productFromDb.ImgUrl,
                Description = productFromDb.Description,
                UserId = productFromDb.UserId
            };

            return product;
        }

        public IQueryable<SystemProductServiceModel> GetAllSystemProducts()
        {
            var products = this.dbContext
                               .SystemProducts
                               .Select(x => new SystemProductServiceModel()
                               {
                                   Name = x.Name,
                                   ImgUrl = x.ImgUrl,
                                   Description = x.Description,
                                   Price = x.Price,
                                   UserId = x.UserId,
                                   Id = x.Id
                               });

            return products;
        }

        //public async Task<bool> ContainsSystemProductAsync(SystemProduct product)
        //{
        //    return await this.dbContext.SystemProducts.ContainsAsync(product);
        //}

        //public async Task<bool> ContainsSystemProductAsync(int id)
        //{
        //    return await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Id == id) != null;
        //}

        //public async Task<bool> ContainsSystemProductAsync(string name)
        //{
        //    return await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Name == name) != null;
        //}

        public async Task<bool> DeleteSystemProductAsync(int id)
        {
            var product = await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Id == id);

            if (product != null)
            {
                this.dbContext.SystemProducts.Remove(product);
                await this.dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateSystemProductByIdAsync(int id, string name, decimal price, string imgUrl, string description)
        {
            var product = await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            product.Name = name;
            product.Price = price;
            product.ImgUrl = imgUrl;
            product.Description = description;

            this.dbContext.SystemProducts.Update(product);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}