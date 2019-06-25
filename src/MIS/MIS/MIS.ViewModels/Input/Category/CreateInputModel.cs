namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CreateInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Address { get; set; }
    }
}