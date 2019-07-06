namespace MIS.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using ServicesModels;

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

        public async Task<ReceiptServiceModel> FinishCurrentOpenReceiptByUsernameAsync(string username)
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

            receipt.IssuedOn = DateTime.UtcNow;
            this.dbContext.Update(receipt);
            await this.dbContext.SaveChangesAsync();

            return receipt.MapTo<ReceiptServiceModel>();
        }
    }
}