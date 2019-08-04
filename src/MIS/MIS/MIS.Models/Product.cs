namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product : BaseModel<string>
    {
        public Product()
        {
            this.ReceiptProducts = new HashSet<ReceiptProduct>();
        }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(typeof(decimal) ,"0.01", "79228162514264337593543950335M")]
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