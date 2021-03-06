﻿namespace MIS.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Mapping;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    using Models;

    public class InvitationService : IInvitationService
    {
        private readonly MISDbContext dbContext;
        private readonly ICompanyService companyService;
        private readonly IUserService userService;

        public InvitationService(MISDbContext dbContext,
            ICompanyService companyService,
            IUserService userService)
        {
            this.dbContext = dbContext;
            this.companyService = companyService;
            this.userService = userService;
        }

        public async Task<IEnumerable<InvitationServiceModel>> GetAllAsync(string id)
        {
            var invitations = await this.dbContext.Invitations
                                        .Where(x => x.UserId == id)
                                        .To<InvitationServiceModel>()
                                        .ToListAsync();

            return invitations;
        }

        public async Task<InvitationServiceModel> InviteAsync(string companyId, string userId)
        {
            var invitation = new Invitation();

            await this.companyService.SetCompanyAsync(invitation, companyId);
            await this.userService.SetInvitationAsync(invitation, userId);

            if (invitation.User == null || invitation.Company == null)
            {
                return null;
            }

            await this.dbContext.AddAsync(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }

        public async Task<InvitationServiceModel> AcceptInvitationAsync(string invitationId, bool isOwner)
        {
            var invitation = await this.dbContext.Invitations
                                       .Include(x => x.Company)
                                       .Include(x => x.User)
                                       .ThenInclude(x => x.Company)
                                       .FirstOrDefaultAsync(x => x.Id == invitationId);
            if (invitation == null)
            {
                return null;
            }

            var user = invitation.User;

            user.Company = invitation.Company;

            this.dbContext.Update(invitation.User);
            this.dbContext.Remove(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }

        public async Task<InvitationServiceModel> DeclineInvitationAsync(string invitationId)
        {
            var invitation = await this.dbContext.Invitations.FirstOrDefaultAsync(x => x.Id == invitationId);

            if (invitation == null)
            {
                return null;
            }

            this.dbContext.Remove(invitation);
            await this.dbContext.SaveChangesAsync();

            return invitation.MapTo<InvitationServiceModel>();
        }
    }
}