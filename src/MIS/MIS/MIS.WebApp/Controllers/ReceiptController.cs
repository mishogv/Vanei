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

    using ViewModels.Input.Receipt;
    using ViewModels.View.Product;
    using ViewModels.View.Receipt;

    public class ReceiptController : AuthenticationController
    {
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
                return this.RedirectToAction("Create", "Company");
            }

            return this.View();
        }

        public async Task<ActionResult<CreateReceiptViewModel>> LoadReceipt()
        {
            var openedReceipt = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(this.User.Identity.Name);
            var result = new CreateReceiptViewModel()
            {
                Username = openedReceipt.User.UserName,
                Products = openedReceipt.ReceiptProducts
                                        .OrderByDescending(x => x.AddedOn)
                                        .Select(x => new ShowReceiptProductViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Product.Name,
                                            Quantity = x.Quantity,
                                            Price = x.Product.Price,
                                            Total = x.Product.Price * (decimal)x.Quantity,
                                            Barcode = x.Product.BarCode,
                                        }).ToList(),
                Total = openedReceipt.ReceiptProducts.Select(x => x.Total).Sum().ToString("f2")
            };

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
        public async Task<ActionResult<ShowReceiptProductViewModel>> AddProduct([FromBody]AddReceiptProductInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var product = await this.receiptService
                                    .AddProductToOpenedReceiptByUsernameAsync(this.User.Identity.Name, input.Id, input.Quantity);

            var result = new ShowReceiptProductViewModel()
            {
                Id = product.Id,
                Name = product.Product.Name,
                Quantity = product.Quantity,
                Price = product.Product.Price,
                Total = product.Product.Price * (decimal)product.Quantity,
                Barcode = product.Product.BarCode,
            };

            return result;
        }

        [HttpGet("/Receipt/AllPorducts")]
        public async Task<ActionResult<IList<ShowReceiptProductViewModel>>> AllProducts()
        {
            var user = await this.userManger.GetUserAsync(this.User);

            var products = await this.productService.GetAllProductsCompanyIdAsync(user.CompanyId);

            return products.ToList();
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

            //TODO : parameter tampering

            var result = new DetailsReceiptViewModel()
            {
                Id = receipt.Id,
                IssuedOn = (DateTime)receipt.IssuedOn,
                Username = receipt.User.UserName,
                Products = receipt.ReceiptProducts.Select(x => x.MapTo<DetailsReceiptProductViewModel>())
            };

            return this.View(result);
        }


        public async Task<IActionResult> DeleteReceipt(int id)
        {
            var receipt = await this.receiptService.DeleteReceiptByIdAsync(id);

            return this.RedirectToAction("Index", "Report");
        }
    }
}