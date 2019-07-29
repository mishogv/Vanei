﻿namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using MIS.Models;

    public class AdministratorService : IAdministratorService
    {
        private readonly UserManager<MISUser> userManager;

        public AdministratorService(UserManager<MISUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> CreateAdministratorByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false; 
            }

            if (user.UserName == GlobalConstants.RootAdminName)
            {
                return false;
            }


            var result = await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorAreaRole);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveAdministratorByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            if (user.UserName == GlobalConstants.RootAdminName)
            {
                return false;
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorAreaRole);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public IQueryable<MISUser> GetAllUsers()
        {
            return this.userManager.Users.Include(x => x.Company);
        }
    }
}