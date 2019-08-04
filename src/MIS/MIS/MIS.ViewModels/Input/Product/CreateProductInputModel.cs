namespace MIS.ViewModels.Input.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Category;

    using Models;

    public class CreateProductInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(0.0001, double.MaxValue)]
        public double Quantity { get; set; }

        [Required]
        public string BarCode { get; set; }

        public IEnumerable<CreateCategoryWareHouseModel> Categories { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string WarehouseId { get; set; }
    }
}