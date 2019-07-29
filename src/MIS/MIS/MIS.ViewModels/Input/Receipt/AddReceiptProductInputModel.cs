namespace MIS.ViewModels.Input.Receipt
{
    using System.ComponentModel.DataAnnotations;

    public class AddReceiptProductInputModel
    {
        [Required]
        public string Id { get; set; }


        [Range(0.001, double.MaxValue)]
        public double Quantity { get; set; }
    }
}