namespace MIS.Models
{
    public class ReceiptProduct : BaseModel<int>
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }

        public decimal Total { get; set; }
    }
}