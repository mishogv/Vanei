namespace MIS.WebApp.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services;
    using Services.Mapping;

    using ViewModels.View.Product;
    using ViewModels.View.Receipt;

    public class ReceiptController : AuthenticationController
    {
        private readonly IReceiptService receiptService;
        private readonly IProductService productService;

        public ReceiptController(IReceiptService receiptService, IProductService productService)
        {
            this.receiptService = receiptService;
            this.productService = productService;
        }

        public async Task<IActionResult> Create()
        {
            //TODO : refactor use automapper
            var openedReceipt = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(this.User.Identity.Name);
            var result = new CreateReceiptViewModel()
            {
                Username = openedReceipt.User.UserName,
                Products = openedReceipt.Products
                                        .OrderByDescending(x => x.AddedOn)
                                        .Select(x => new ShowReceiptProductViewModel
                {
                    Id = x.Id,
                    Name = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Product.Price,
                    Total = x.Product.Price * (decimal) x.Quantity,
                    Barcode = x.Product.BarCode,
                }).ToList(),
                Total = openedReceipt.Products.Select(x => x.Total).Sum()
            };

            result.Products.Add(new ShowReceiptProductViewModel());

            return this.View(result);
        }

        public IActionResult Delete()
        {
            // user
            // find opened receipt
            // load data

            return View();
        }

        public IActionResult AddProduct(/*AddReceiptProductInputModel input*/)
        {
            // AJAX
            // user
            // find opened receipt
            // Add product to opened receipt 
            // return added product
            // security ??

            return View();
        }

        [HttpGet("/All/Products")]
        public async Task<ActionResult<IList<ShowReceiptProductViewModel>>> AllProducts()
        {
            // AJAX
            // user
            // find opened receipt
            // Add product to opened receipt 
            // return added product
            // security ??
            var products = await this.productService.GetAllProductsByUsernameAsync(this.User.Identity.Name);

            return products.ToList();
        }

        public IActionResult RemoveProduct()
        {
            // AJAX
            // user
            // find opened receipt
            // remove from opened receipt 
            // delete product to frontend

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Finish()
        {
            //TODO : validate is working correctly
            await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(this.User.Identity.Name);

            return this.RedirectToAction("Create");
        }
    }
}