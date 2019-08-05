namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category : BaseModel<string>
    {
        private const int MinLength = 2;
        private const int MaxLength = 24;

        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        [Required]
        public string WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }
    }
}