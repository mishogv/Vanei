namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseModel<string>
    {
        private const int MinLength = 3;
        private const int MaxLength = 24;

        private const string DecimalMinValue = "0.01";
        private const string DecimalMaxValue = "79228162514264337593543950335";

        public Product()
        {
            this.ReceiptProducts = new HashSet<ReceiptProduct>();
        }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        [Range(typeof(decimal) ,DecimalMinValue, DecimalMaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity in warehouse
        /// </summary>
        [Range(double.MinValue, double.MaxValue)]
        public double Quantity { get; set; }

        [Required]
        public string BarCode { get; set; }

        [Required]
        public string CategoryId { get; set; } 
        public virtual Category Category { get; set; }

        [Required]
        public string WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }

        public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; }
    }
}