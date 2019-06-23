namespace MIS.ServicesModels
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class ReceiptServiceModel
    {
        public ReceiptServiceModel()
        {
            this.Products = new HashSet<Product>();
        }

        public DateTime IssuedOn { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }
        public MISUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}