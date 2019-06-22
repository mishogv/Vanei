namespace MIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Receipt : BaseModel<int>
    {
        public Receipt()
        {
            this.Products = new HashSet<Product>();
        }

        [Required]
        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }

        public MISUser User { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}