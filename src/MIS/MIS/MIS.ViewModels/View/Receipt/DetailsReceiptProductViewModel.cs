﻿namespace MIS.ViewModels.View.Receipt
{
    using System;

    using Models;

    using Services.Mapping;

    public class DetailsReceiptProductViewModel : IMapFrom<ReceiptProduct>
    {
        public string ProductName { get; set; }

        public string Barcode { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductCategoryName { get; set; }

        public DateTime AddedOn { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }
    }
}