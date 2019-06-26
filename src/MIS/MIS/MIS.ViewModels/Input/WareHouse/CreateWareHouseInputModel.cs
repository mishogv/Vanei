namespace MIS.ViewModels.Input.WareHouse
{
    using System.ComponentModel.DataAnnotations;

    public class CreateWareHouseInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 3)]
        public string Name { get; set; }
    }
}