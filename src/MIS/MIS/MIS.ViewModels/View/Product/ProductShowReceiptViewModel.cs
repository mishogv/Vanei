﻿namespace MIS.ViewModels.View.Product
{
    using Models;

    using Services.Mapping;

    public class ProductShowReceiptViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }

        public decimal Price { get; set; }

        public string Barcode { get; set; }
    }
}