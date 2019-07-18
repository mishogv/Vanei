namespace MIS.ServicesModels
{
    using System;

    using Models;

    using Services.Mapping;

    public class ReceiptProductServiceModel : IMapFrom<ReceiptProduct>
    {
        public int Id { get; set; }

        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }

        public DateTime AddedOn { get; set; }

        public decimal Total { get; set; }  
    }
}