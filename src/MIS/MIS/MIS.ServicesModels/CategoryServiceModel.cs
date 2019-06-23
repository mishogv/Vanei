namespace MIS.ServicesModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Models;

    public class CategoryServiceModel
    {
        public CategoryServiceModel()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public int WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}