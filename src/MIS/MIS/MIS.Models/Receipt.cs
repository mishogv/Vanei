namespace MIS.Models
{
    using System;
    using System.Collections.Generic;

    public class Receipt : BaseModel<int>
    {
        public Receipt()
        {
            this.ReceiptProducts = new HashSet<ReceiptProduct>();
        }

        public DateTime? IssuedOn { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; }
    }
}