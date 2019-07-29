﻿using Microsoft.AspNetCore.Mvc;

namespace MIS.WebApp.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Hubs;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    using Models;

    using Services;
    using Services.Mapping;

    using ViewModels.Input.Company;
    using ViewModels.View.Chat;
    using ViewModels.View.Company;

    public class CompanyController : AuthenticationController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<MISUser> userManager;
        private readonly SignInManager<MISUser> signInManager;
        private readonly IMessageService messageService;

        public CompanyController(ICompanyService companyService,
            UserManager<MISUser> userManager,
            SignInManager<MISUser> signInManager,
            IMessageService messageService)
        {
            this.companyService = companyService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction(nameof(this.Create));
            }

            var company = await this.companyService.GetCompanyAsync(user.CompanyId);

            var employees = company.Employees.Select(x => x.MapTo<DetailsCompanyUserViewModel>());

            var result = company.MapTo<DetailsCompanyViewModel>();

            return this.View(result);
        }

        public async Task<IActionResult> Chat(string id)
        {
            var messages = await this.messageService.GetAll(id);
            var result = new CompanyChatViewModel
            {
                Id = id,
                Messages = messages.MapTo<ChatHubMessageViewModel[]>()
            };

            return this.View(result);
        }

        public IActionResult Create()
        {
            return this.View(new CreateCompanyInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.userManager.AddToRoleAsync(user, GlobalConstants.CompanyOwnerRole);
            await this.companyService.CreateAsync(input.Name, input.Address, user.Id);
            await this.signInManager.SignOutAsync();

            this.TempData[GlobalConstants.CompanyOwnerRole] = "You have to log in asdqwd";

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}