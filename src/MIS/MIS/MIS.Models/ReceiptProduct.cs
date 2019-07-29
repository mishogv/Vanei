namespace MIS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReceiptProduct : BaseModel<string>
    {
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        [Required]
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [Range(0, double.MaxValue)]
        public double Quantity { get; set; }

        public DateTime AddedOn { get; set; }

        [Range(typeof(decimal), "0.000", "79228162514264337593543950335")]
        public decimal Total { get; set; }
    }
}