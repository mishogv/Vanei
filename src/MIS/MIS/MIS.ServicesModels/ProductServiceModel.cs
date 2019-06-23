namespace MIS.ServicesModels
{
    using Models;

    public class ProductServiceModel
    {
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