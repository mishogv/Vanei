namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCategoryInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }
    }
}