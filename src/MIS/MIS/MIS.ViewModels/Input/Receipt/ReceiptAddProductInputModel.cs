namespace MIS.ViewModels.Input.Receipt
{
    using System.ComponentModel.DataAnnotations;

    public class ReceiptAddProductInputModel
    {
        private const double DoubleMinValue = 0.0001;

        [Required]
        public string Id { get; set; }


        [Range(DoubleMinValue, double.MaxValue)]
        public double Quantity { get; set; }
    }
}