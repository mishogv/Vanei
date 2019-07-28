﻿namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;

    using Services.Models;

    using ViewModels.Input.Category;
    using ViewModels.View.Category;

    public class CategoryController : AuthenticationController
    {
        private readonly ICategoryService categoryService;
        private readonly UserManager<MISUser> userManager;

        public CategoryController(ICategoryService categoryService, UserManager<MISUser> userManager)
        {
            this.categoryService = categoryService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            //TODO IF NULL
            var categories = await this.categoryService.GetAllByCompanyIdAsync((int) user.CompanyId);

            var categoryServiceModels = categories as CategoryServiceModel[] ?? categories.ToArray();
            var categoriesViewModels = new List<CategoryIndexDetailsViewModel>();

            foreach (var category in categoryServiceModels)
            {
                var categoryViewModel = category.MapTo<CategoryIndexDetailsViewModel>();
                categoryViewModel.ProductsCount = category.Products.Count;

                categoriesViewModels.Add(categoryViewModel);
            }

            var result = new CategoryIndexViewModel()
            {
                Categories = categoriesViewModels
            };

            return this.View(result);
        }


        public IActionResult Create(int id)
        {
            return this.View(new CreateCategoryInputModel { Id = id});
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel categoryInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryInput);
            }

            //TODO : VALIDATE AND SECURITY

            await this.categoryService.CreateAsync(categoryInput.Name, categoryInput.Id);

            return this.RedirectToAction("Index", "WareHouse");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoryService.GetCategoryAsync(id);

            return this.View(category.MapTo<EditCategoryInputModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoryService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //TODO PARAMTER TAMPERING
            var category = await this.categoryService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}