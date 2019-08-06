namespace MIS.ViewModels.View.Product
{
    using Services.Mapping;
    using Services.Models;

    public class ProductReceiptViewModel : IMapFrom<ProductServiceModel>
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public string Barcode { get; set; }

        public string CategoryName { get; set; }
    }
}