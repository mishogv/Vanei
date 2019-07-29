namespace MIS.Services.Models
{
    using System.Collections.Generic;

    using MIS.Models;

    using Mapping;

    public class CategoryServiceModel : IMapFrom<Category>
    {
        public CategoryServiceModel()
        {
            this.Products = new HashSet<Product>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public string WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}