namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class ReceiptService : IReceiptService
    {
        private readonly MISDbContext dbContext;
        private readonly IUserService userService;
        private readonly ICompanyService companyService;
        private readonly IProductService productService;

        public ReceiptService(MISDbContext dbContext,
            IUserService userService, 
            ICompanyService companyService,
            IProductService productService)
        {
            this.dbContext = dbContext;
            this.userService = userService;
            this.companyService = companyService;
            this.productService = productService;
        }

        public async Task<ReceiptServiceModel> GetCurrentOpenedReceiptByUsernameAsync(string username)
        {
            var receipt = await this.dbContext.Receipts
                              .Include(x => x.User)
                              .Include(x => x.ReceiptProducts)
                              .ThenInclude(x => x.Product)
                              .Where(x => x.User.UserName == username && x.IssuedOn == null)
                              .FirstOrDefaultAsync();

            if (receipt == null)
            {
                receipt = new Receipt();

                var companyId = await this.userService.SetReceiptAsync(receipt, username);
                await this.companyService.SetCompanyAsync(receipt, companyId);

                await this.dbContext.AddAsync(receipt);
                await this.dbContext.SaveChangesAsync();
            }

            var result = receipt.MapTo<ReceiptServiceModel>();

            return result;
        }

        public async Task<ReceiptProductServiceModel> AddProductToOpenedReceiptByUsernameAsync(string username, string productId, double quantity)
        {
            var receipt = await this.dbContext.Receipts
                                    .Include(x => x.User)
                                    .Include(x => x.ReceiptProducts)
                                    .ThenInclude(x => x.Product)
                                    .Where(x => x.User.UserName == username && x.IssuedOn == null)
                                    .FirstOrDefaultAsync();

            if (receipt == null)
            {
                return null;
            }

            var receiptProduct = new ReceiptProduct
            {
                AddedOn = DateTime.UtcNow,
                Receipt = receipt,
                Quantity = quantity,
            };

            await this.productService.SetProductAsync(receiptProduct, productId);

            receiptProduct.Total = (decimal) quantity * receiptProduct.Product.Price;

            await this.dbContext.AddAsync(receiptProduct);
            await this.dbContext.SaveChangesAsync();

            return receiptProduct.MapTo<ReceiptProductServiceModel>();
        }

        public async Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username)
        {
            var receipt = await this.dbContext.Receipts
                                    .Include(x => x.User)
                                    .Include(x => x.ReceiptProducts)
                                    .ThenInclude(x => x.Product)
                                    .Where(x => x.User.UserName == username && x.IssuedOn == null)
                                    .FirstOrDefaultAsync();

            if (receipt == null || receipt.ReceiptProducts.Count == 0)
            {
                return null;
            }

            var products = new List<Product>();

            foreach (var receiptProduct in receipt.ReceiptProducts)
            {
                receipt.Total += receiptProduct.Total;
                receiptProduct.Product.Quantity -= receiptProduct.Quantity;
                products.Add(receiptProduct.Product);
            }

            receipt.IssuedOn = DateTime.UtcNow;
            this.dbContext.UpdateRange(products);
            this.dbContext.Update(receipt);
            await this.dbContext.SaveChangesAsync();

            return receipt.MapTo<ReceiptServiceModel>();
        }

        public async Task<ReceiptServiceModel> DeleteReceiptAsync(string username)
        {
            var receipt = await this.dbContext.Receipts
                                    .Include(x => x.User)
                                    .Include(x => x.ReceiptProducts)
                                    .ThenInclude(x => x.Product)
                                    .Where(x => x.User.UserName == username && x.IssuedOn == null)
                                    .FirstOrDefaultAsync();

            if (receipt == null)
            {
                return null;
            }

            receipt.ReceiptProducts.Clear();

            this.dbContext.Update(receipt);
            await this.dbContext.SaveChangesAsync();

            return receipt.MapTo<ReceiptServiceModel>();
        }

        public async Task<ReceiptServiceModel> GetReceiptAsync(int id)
        {
          var receipt =  await this.dbContext
                .Receipts
                .Include(x => x.User)
                .Include(x => x.ReceiptProducts)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

          return receipt.MapTo<ReceiptServiceModel>();
        }

        public async Task<ReceiptServiceModel> DeleteReceiptByIdAsync(int id)
        {
            var receipt = await this.dbContext.Receipts
                                    .Include(x => x.ReceiptReports)
                                    .Include(x => x.ReceiptProducts)
                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (receipt == null)
            {
                return null;
            }

            this.dbContext.RemoveRange(receipt.ReceiptReports);
            this.dbContext.RemoveRange(receipt.ReceiptProducts);
            this.dbContext.Remove(receipt);
            await this.dbContext.SaveChangesAsync();

            return receipt.MapTo<ReceiptServiceModel>();
        }

        public async Task<IEnumerable<ReceiptServiceModel>> SetReceiptsAsync(Report report, DateTime from, DateTime to, string companyId)
        {
            var receipts = await this.dbContext.Receipts
                                     .Where(x => x.CompanyId == companyId)
                                     .Where(x => x.IssuedOn >= from && x.IssuedOn <= to)
                                     .ToListAsync();

            foreach (var receipt in receipts)
            {
                report.ReceiptReports.Add(new ReceiptReport()
                {
                    Receipt = receipt
                });
            }

            return receipts.MapTo<ReceiptServiceModel[]>();
        }
    }
}