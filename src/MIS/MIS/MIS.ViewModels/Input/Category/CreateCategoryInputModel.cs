namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCategoryInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }
        
        [Required]
        public string WareHouseName { get; set; }
    }
}