namespace MIS.Services.Models
{
    using MIS.Models;

    using Models;

    using Services.Mapping;

    public class ProductServiceModel : IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public double Quantity { get; set; }

        public string BarCode { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}