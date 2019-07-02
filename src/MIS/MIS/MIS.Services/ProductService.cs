namespace MIS.Services
{
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Models;

    using ServicesModels;

    public class ProductService : IProductService
    {
        private readonly MISDbContext db;

        public ProductService(MISDbContext db)
        {
            this.db = db;
        }

        public async Task<ProductServiceModel> Create(string name, decimal price, double quantity, string barcode)
        {
            var product = new Product
            {
                Name = name,
                BarCode = barcode,
                Price = price,
                Quantity = quantity,
            };

           await  this.db.AddAsync(product);
           await this.db.SaveChangesAsync();

           return product.MapTo<ProductServiceModel>();
        }
    }
}