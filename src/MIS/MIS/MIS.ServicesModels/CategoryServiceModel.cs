namespace MIS.ServicesModels
{
    using System.Collections.Generic;

    using Models;

    using Services.Mapping;

    public class CategoryServiceModel : IMapFrom<Category>
    {
        public CategoryServiceModel()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}