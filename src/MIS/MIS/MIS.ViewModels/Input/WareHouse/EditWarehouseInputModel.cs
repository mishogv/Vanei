namespace MIS.ViewModels.Input.WareHouse
{
    using System.ComponentModel.DataAnnotations;

    using Services.Mapping;

    using Services.Models;

    public class EditWarehouseInputModel : IMapFrom<WareHouseServiceModel>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }
    }
}