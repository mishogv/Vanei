﻿namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;

    using ViewModels.AdministratorManage;

    public class AdministratorManageController : AdministratorController
    {
        private readonly IAdministratorService administratorService;

        public AdministratorManageController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        public async Task<IActionResult> Index()
        {
            //TODO : Walk around for exception, refactor it!

            var dict = new Dictionary<MISUser, string>();
            var users = this.administratorService
                            .GetAllUsers()
                            .ToList();

            foreach (var user in users)
            {
                var role = await this.administratorService.GetUserRoleAsync(user);
                var roleToAdd = role ?? "User";
                dict.Add(user, roleToAdd);
            }

            var result = new List<AdministratorShowUserViewModel>();

            foreach (var kvp in dict)
            {
                result.Add(new AdministratorShowUserViewModel()
                {
                    Id = kvp.Key.Id,
                    LastName = kvp.Key.LastName,
                    FirstName = kvp.Key.FirstName,
                    Email = kvp.Key.Email,
                    Role = kvp.Value,
                    PhoneNumber = kvp.Key.PhoneNumber ?? "N/A",
                    CompanyName = kvp.Key.Company == null ? "N/A" : kvp.Key.Company.Name,
                    CompanyRole = kvp.Key.Role.ToString("G"),
                    Username = kvp.Key.UserName
                });
            }

            return this.View(result);
        }

        public async Task<IActionResult> Create(string id)
        {
            var result = await this.administratorService.CreateAdministratorByIdAsync(id);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string id)
        {
            var result = await this.administratorService.RemoveAdministratorByIdAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}