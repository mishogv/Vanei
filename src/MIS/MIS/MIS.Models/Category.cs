namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category : BaseModel<string>
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [Required]
        public string WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}