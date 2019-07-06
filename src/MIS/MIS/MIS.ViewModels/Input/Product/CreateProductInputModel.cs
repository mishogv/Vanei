namespace MIS.ViewModels.Input.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Models;

    public class CreateProductInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public double Quantity { get; set; }

        [Required]
        [RegularExpression(@"^8[0-9]{11}([0-9]{2})?$")]
        public string BarCode { get; set; }

        public IEnumerable<string> CategoryNames { get; set; }

        [Required]
        public string WareHouseName { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}