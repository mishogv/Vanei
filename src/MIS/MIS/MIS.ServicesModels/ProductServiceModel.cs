namespace MIS.Services.Models
{
    using MIS.Models;

    using Models;

    using Services.Mapping;

    public class ProductServiceModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string BarCode { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}