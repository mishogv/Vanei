namespace MIS.Models
{
    public class ReceiptReport : BaseModel<int>
    {
        public Receipt Receipt { get; set; }
        public int ReceiptId { get; set; }

        public Report Report { get; set; }
        public int ReportId { get; set; }
    }
}