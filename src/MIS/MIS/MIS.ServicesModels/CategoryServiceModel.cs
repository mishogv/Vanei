namespace MIS.ServicesModels
{
    using System.Collections.Generic;

    using Models;

    public class CategoryServiceModel
    {
        public CategoryServiceModel()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}