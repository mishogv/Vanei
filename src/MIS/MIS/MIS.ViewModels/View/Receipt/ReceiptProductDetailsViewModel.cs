namespace MIS.ViewModels.View.Receipt
{
    using System;

    using Models;

    using Services.Mapping;

    public class ReceiptProductDetailsViewModel : IMapFrom<ReceiptProduct>
    {
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductCategoryName { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }
    }
}