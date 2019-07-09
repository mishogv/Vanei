namespace MIS.ViewModels.View.Product
{
    using Models;

    using Services.Mapping;

    public class WareHouseIndexProductViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string BarCode { get; set; }

        public string CategoryName { get; set; }
    }
}