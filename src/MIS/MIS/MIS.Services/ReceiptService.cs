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

        public ReceiptService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReceiptServiceModel> GetCurrentOpenedReceiptByUsernameAsync(string username)
        {
            var user = await this.dbContext.Users
                           .Include(x => x.Receipts)
                           .ThenInclude(x => x.ReceiptProducts)
                           .ThenInclude(x => x.Product)
                           .Include(x => x.Company)
                           .FirstOrDefaultAsync(x => x.UserName == username);

            var receipt = user.Receipts.FirstOrDefault(x => x.IssuedOn == null);

            if (receipt == null)
            {
                receipt = new Receipt()
                {
                    Company = user.Company,
                    User = user
                };

                await this.dbContext.AddAsync(receipt);

                await this.dbContext.SaveChangesAsync();
            }

            var result = receipt.MapTo<ReceiptServiceModel>();

            return result;
        }

        public async Task<ReceiptProductServiceModel> AddProductToOpenedReceiptByUsernameAsync(string username, int id, double quantity)
        {
            var user = await this.dbContext.Users
                                 .Include(x => x.Receipts)
                                 .Include(x => x.Company)
                                 .FirstOrDefaultAsync(x => x.UserName == username);

            var receipt = user.Receipts.FirstOrDefault(x => x.IssuedOn == null);

            if (receipt == null)
            {
                return null;
            }

            var product = await this.dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            var receiptProduct = new ReceiptProduct
            {
                AddedOn = DateTime.UtcNow,
                Receipt = receipt,
                Product = product,
                Quantity = quantity,
                Total = (decimal) quantity * product.Price
            };

            await this.dbContext.AddAsync(receiptProduct);
            await this.dbContext.SaveChangesAsync();

            return receiptProduct.MapTo<ReceiptProductServiceModel>();
        }

        public async Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username)
        {
            var user = await this.dbContext.Users
                                 .Include(x => x.Receipts)
                                 .ThenInclude(x => x.ReceiptProducts)
                                 .ThenInclude(x => x.Product)
                                 .Include(x => x.Company)
                                 .FirstOrDefaultAsync(x => x.UserName == username);

            var receipt = user.Receipts.FirstOrDefault(x => x.IssuedOn == null);

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
            var user = await this.dbContext.Users
                                 .Include(x => x.Receipts)
                                 .ThenInclude(x => x.ReceiptProducts)
                                 .ThenInclude(x => x.Product)
                                 .Include(x => x.Company)
                                 .FirstOrDefaultAsync(x => x.UserName == username);

            var receipt = user.Receipts.FirstOrDefault(x => x.IssuedOn == null);

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

            this.dbContext.RemoveRange(receipt.ReceiptReports);
            this.dbContext.RemoveRange(receipt.ReceiptProducts);
            //TODO : if null

            this.dbContext.Remove(receipt);
            await this.dbContext.SaveChangesAsync();

            return receipt.MapTo<ReceiptServiceModel>();
        }

        public async Task<IEnumerable<ReceiptServiceModel>> SetReceiptsAsync(Report report, DateTime from, DateTime to, int companyId)
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