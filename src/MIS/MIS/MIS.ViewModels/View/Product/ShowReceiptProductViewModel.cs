namespace MIS.ViewModels.View.Product
{
    using Models;

    using Services.Mapping;

    using ServicesModels;

    public class ShowReceiptProductViewModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }

        public decimal Price { get; set; }

        public string Barcode { get; set; }
    }
}