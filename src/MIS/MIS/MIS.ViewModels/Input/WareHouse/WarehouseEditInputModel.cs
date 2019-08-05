namespace MIS.ViewModels.Input.WareHouse
{
    using System.ComponentModel.DataAnnotations;

    using Services.Mapping;

    using Services.Models;

    public class WarehouseEditInputModel : IMapFrom<WareHouseServiceModel>
    {
        private const int MinLength = 3;
        private const int MaxLength = 24;

        public string Id { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }
    }
}