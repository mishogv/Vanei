﻿namespace MIS.Services.Models
{
    using System;

    using MIS.Models;

    using Mapping;

    public class ReceiptProductServiceModel : IMapFrom<ReceiptProduct>
    {
        public string Id { get; set; }

        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }

        public DateTime AddedOn { get; set; }

        public decimal Total { get; set; }  
    }
}