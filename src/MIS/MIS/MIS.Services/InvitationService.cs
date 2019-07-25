namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Mapping;

    using Microsoft.EntityFrameworkCore;

    using ServicesModels;

    public class InvitationService : IInvitationService
    {
        private readonly MISDbContext dbContext;

        public InvitationService(MISDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id)
        {
            var invitations = await this.dbContext.Invitations.Where(x => x.UserId == id)
                                  .To<InvitationServiceModel>()
                                  .ToListAsync();

            return invitations;
        }
    }
}