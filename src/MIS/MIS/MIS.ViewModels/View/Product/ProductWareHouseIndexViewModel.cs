namespace MIS.ViewModels.View.Product
{
    using Models;

    using Services.Mapping;

    public class ProductWareHouseIndexViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string BarCode { get; set; }

        public string CategoryName { get; set; }
    }
}