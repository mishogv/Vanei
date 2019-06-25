namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class WareHouse : BaseModel<int>
    {
        public WareHouse()
        {
            this.Categories = new HashSet<Category>();
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}