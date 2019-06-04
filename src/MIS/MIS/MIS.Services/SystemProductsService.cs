namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public class SystemProductsService : ISystemProductsService
    {
        private readonly MISDbContext dbContext;

        public SystemProductsService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SystemProduct> CreateSystemProductAsync(string name, decimal price, string imgUrl, string description, string userId)
        {
            //TODO: Validate?????
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

            return systemProduct;
        }

        public async Task<SystemProduct> GetSystemProductByIdAsync(int id)
        {
            //TODO: Validate?????
            var product = await this.dbContext.SystemProducts.FindAsync(id);


            return product;
        }

        public async Task<IEnumerable<SystemProduct>> GetAllSystemProductsAsync()
        {
            //TODO: Validate?????
            var products = await this.dbContext.SystemProducts.ToListAsync();

            return products;
        }

        public async Task<bool> ContainsSystemProductAsync(SystemProduct product)
        {
            return await this.dbContext.SystemProducts.ContainsAsync(product);
        }

        public async Task<bool> ContainsSystemProductAsync(int id)
        {
            return await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Id == id) != null;
        }

        public async Task<bool> ContainsSystemProductAsync(string name)
        {
            return await this.dbContext.SystemProducts.FirstOrDefaultAsync(x => x.Name == name) != null;
        }

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

        public async Task<bool> UpdateSystemProductAsync(int id, string name, decimal price, string imgUrl, string description, string userId)
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
            product.UserId = userId;

            this.dbContext.SystemProducts.Update(product);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}