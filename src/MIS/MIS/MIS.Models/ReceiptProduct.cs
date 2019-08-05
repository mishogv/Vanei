namespace MIS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReceiptProduct : BaseModel<string>
    {
        private const string DecimalMinValue = "0.01";
        private const string DecimalMaxValue = "79228162514264337593543950335";

        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        [Required]
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [Range(0, double.MaxValue)]
        public double Quantity { get; set; }

        public DateTime AddedOn { get; set; }

        [Range(typeof(decimal), DecimalMinValue, DecimalMaxValue)]
        public decimal Total { get; set; }
    }
}