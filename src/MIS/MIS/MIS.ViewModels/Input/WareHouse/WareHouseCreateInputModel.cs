namespace MIS.ViewModels.Input.WareHouse
{
    using System.ComponentModel.DataAnnotations;

    public class WareHouseCreateInputModel
    {
        private const int MinLength = 3;
        private const int MaxLength = 24;

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }
    }
}