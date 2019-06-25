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
            this.IssuedOn = DateTime.UtcNow;
        }

        //TODO : Maybe make bug DONT FORGET THAT
        [Required]
        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}