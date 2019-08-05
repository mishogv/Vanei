namespace MIS.ViewModels.Input.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Category;

    using Services.Mapping;

    using Services.Models;

    public class EditProductInputModel : IMapFrom<ProductServiceModel>
    {
        private const int MinLength = 3;
        private const int MaxLength = 24;

        private const double DoubleMinValue = 0.0001;
        private const string DecimalMinValue = "0.01";
        private const string DecimalMaxValue = "79228162514264337593543950335";

        public string Id { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        [Range(typeof(decimal), DecimalMinValue, DecimalMaxValue)]
        public decimal Price { get; set; }

        [Range(DoubleMinValue, double.MaxValue)]
        public double Quantity { get; set; }

        [Required]
        public string BarCode { get; set; }

        public IEnumerable<CategoryCreateWareHouseInputModel> Categories { get; set; }

        [Required]
        public string CategoryId { get; set; }
    }
}