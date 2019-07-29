﻿namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class WareHouse : BaseModel<string>
    {
        public WareHouse()
        {
            this.Categories = new HashSet<Category>();
            this.Products = new HashSet<Product>();
        }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }

        public bool IsFavorite { get; set; }

        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}