namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryCreateInputModel
    {
        private const int MinLength = 2;
        private const int MaxLength = 24;

        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }
    }
}