namespace MIS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class MessageService : IMessageService
    {
        private readonly MISDbContext dbContext;
        private readonly ICompanyService companyService;

        public MessageService(MISDbContext dbContext, ICompanyService companyService)
        {
            this.dbContext = dbContext;
            this.companyService = companyService;
        }

        public async Task<MessageServiceModel> CreateAsync(string companyId, string username, string text, bool isNotification)
        {
            var message = new Message()
            {
                Username = username,
                AddedOn =  DateTime.UtcNow,
            };

            await this.companyService.SetCompanyAsync(message, companyId);

            message.Text = isNotification ? string.Format(text, message.Username, message.Company.Name) : text;

            await this.dbContext.Messages.AddAsync(message);
            await this.dbContext.SaveChangesAsync();

            return message.MapTo<MessageServiceModel>();
        }

        public async Task<IEnumerable<MessageServiceModel>> GetAll(string companyId)
        {
            var messages = this.dbContext.Messages
                               .Where(x => x.CompanyId == companyId)
                               .OrderByDescending(x => x.AddedOn)
                               .Take(20);
            
            return await messages
                             .To<MessageServiceModel>()
                             .ToListAsync();
        }
    }
}