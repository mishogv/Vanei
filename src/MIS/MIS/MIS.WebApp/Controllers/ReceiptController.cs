namespace MIS.WebApp.Controllers
{
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

        public ReceiptController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        public async Task<IActionResult> Create()
        {
            var openedReceipt = await this.receiptService.GetCurrentOpenedReceiptByUsernameAsync(this.User.Identity.Name);
            var result = new CreateReceiptViewModel()
            {
                Username = openedReceipt.User.UserName,
                Products = openedReceipt.Products
                                        .OrderByDescending(x => x.AddedOn)
                                        .Select(x => new ShowReceiptProductViewModel
                {
                    Name = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Product.Price,
                    Total = x.Product.Price * (decimal) x.Quantity,
                    Id = x.Id,
                    Barcode = x.Product.BarCode,
                }).ToList(),
                Total = openedReceipt.Products.Select(x => x.Total).Sum()
            };

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
            await this.receiptService.FinishCurrentOpenReceiptByUsernameAsync(this.User.Identity.Name);

            return this.RedirectToAction("Create");
        }
    }
}