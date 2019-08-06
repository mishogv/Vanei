namespace MIS.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using Services;
    using Services.Mapping;
    using Services.Models;

    using ViewModels.Input.Receipt;
    using ViewModels.View.Product;
    using ViewModels.View.Receipt;

    public class ReceiptController : AuthenticationController
    {
        private const string RedirectCreate = "Create";
        private const string RedirectCompany = "Company";

        private const string RedirectIndex = "Index";
        private const string RedirectReport = "Report";

        private readonly IReceiptService receiptService;
        private readonly IProductService productService;
        private readonly UserManager<MISUser> userManger;

        public ReceiptController(IReceiptService receiptService,
            IProductService productService,
            UserManager<MISUser> userManger)
        {
            this.receiptService = receiptService;
            this.productService = productService;
            this.userManger = userManger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManger.GetUserAsync(this.User);

            if (user.CompanyId == null)
            {
                return this.RedirectToAction(RedirectCreate, RedirectCompany);
            }

            return this.View();
        }

        public async Task<ActionResult<ReceiptCreateViewModel>> LoadReceipt()
        {
            var openedReceipt = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(this.User.Identity.Name) ??
                                await this.receiptService.CreateAsync(this.User.Identity.Name);

            var result = this.GetViewModel(openedReceipt);

            return result;
        }


        [HttpGet("/Receipt/Delete")]
        public async Task<ActionResult> Delete()
        {
            await this.receiptService.DeleteReceiptAsync(this.User.Identity.Name);
            return this.Ok();
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("/Receipt/Add")]
        public async Task<ActionResult<ProductShowReceiptViewModel>> AddProduct([FromBody]ReceiptAddProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var product = await this.receiptService
                                    .AddProductToOpenedReceiptByUsernameAsync(this.User.Identity.Name, input.Id, input.Quantity);

            if (product == null)
            {
                return this.BadRequest(this.ModelState);
            }

            return product.MapTo<ProductShowReceiptViewModel>();
        }

        [HttpGet("/Receipt/AllPorducts")]
        public async Task<ActionResult<IList<ProductReceiptViewModel>>> AllProducts()
        {
            var user = await this.userManger.GetUserAsync(this.User);

            var products = await this.productService.GetAllProductsCompanyIdAsync(user.CompanyId);

            return products.MapTo<ProductReceiptViewModel[]>();
        }


        [HttpGet]
        public async Task<ActionResult> Finish()
        {
            await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(this.User.Identity.Name);

            return this.Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var receipt = await this.receiptService.GetReceiptAsync(id);

            var userId = this.userManger.GetUserId(this.User);

            if (receipt == null || receipt.UserId != userId)
            {
                return this.Forbid();
            }

            var result = new ReceiptDetailsViewModel()
            {
                Id = receipt.Id,
                IssuedOn = (DateTime)receipt.IssuedOn,
                Username = receipt.User.UserName,
                Products = receipt.ReceiptProducts.MapTo<ReceiptProductDetailsViewModel[]>()
            };

            return this.View(result);
        }


        public async Task<IActionResult> DeleteReceipt(int id)
        {
            var userId = this.userManger.GetUserId(this.User);
            var receipt = await this.receiptService.GetReceiptAsync(id);

            if (receipt == null || receipt.UserId != userId)
            {
                return this.Forbid();
            }

            await this.receiptService.DeleteReceiptByIdAsync(id);
            return this.RedirectToAction(RedirectIndex, RedirectReport);
        }


        private ReceiptCreateViewModel GetViewModel(ReceiptServiceModel openedReceipt)
        {
            var result = new ReceiptCreateViewModel()
            {
                Username = openedReceipt.User.UserName,
                Products = openedReceipt.ReceiptProducts
                                        .OrderByDescending(x => x.AddedOn)
                                        .MapTo<ProductShowReceiptViewModel[]>()
                                        .ToList(),
            };

            result.Total = result.Products.Select(x => x.Total).Sum().ToString("f2");
            return result;
        }
    }
}