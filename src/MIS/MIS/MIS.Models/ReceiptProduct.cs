namespace MIS.Models
{
    using System;

    public class ReceiptProduct : BaseModel<int>
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        public decimal Total { get; set; }
    }
}