namespace MIS.Services.Models
{
    using System.Collections.Generic;

    using MIS.Models;

    using Mapping;

    public class WareHouseServiceModel : IMapFrom<WareHouse>, IMapTo<WareHouse>
    {
        public WareHouseServiceModel()
        {
            this.Categories = new HashSet<Category>();
            this.Products = new HashSet<Product>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsFavorite { get; set; }

        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}