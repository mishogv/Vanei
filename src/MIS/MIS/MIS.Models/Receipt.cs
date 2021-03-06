﻿namespace MIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class Receipt : BaseModel<int>, IHaveCompany
    {
        public Receipt()
        {
            this.ReceiptProducts = new HashSet<ReceiptProduct>();
            this.ReceiptReports = new HashSet<ReceiptReport>();
        }

        public DateTime? IssuedOn { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<ReceiptReport> ReceiptReports { get; set; }

        public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; }
    }
}