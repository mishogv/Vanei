﻿namespace MIS.Services.Models
{
    using System;
    using System.Collections.Generic;

    using MIS.Models;

    using Mapping;

    public class ReceiptServiceModel : IMapFrom<Receipt>
    {
        public ReceiptServiceModel()
        {
            this.ReceiptProducts = new HashSet<ReceiptProduct>();
        }

        public int Id { get; set; }

        public DateTime? IssuedOn { get; set; }

        public decimal Total { get; set; } 

        public string UserId { get; set; }
        public MISUser User { get; set; }

        public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; }
    }
}