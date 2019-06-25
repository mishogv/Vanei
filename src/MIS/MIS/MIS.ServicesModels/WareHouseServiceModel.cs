﻿namespace MIS.ServicesModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Models;

    public class WareHouseServiceModel
    {
        public WareHouseServiceModel()
        {
            this.Categories = new HashSet<Category>();
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}