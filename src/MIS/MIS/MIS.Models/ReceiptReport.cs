namespace MIS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ReceiptReport : BaseModel<string>
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        [Required]
        public string ReportId { get; set; }
        public Report Report { get; set; }
    }
}