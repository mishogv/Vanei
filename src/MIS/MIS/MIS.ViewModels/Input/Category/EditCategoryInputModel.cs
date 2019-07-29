namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    using Services.Mapping;

    using Services.Models;

    public class EditCategoryInputModel : IMapFrom<CategoryServiceModel>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 2)]
        public string Name { get; set; }
    }
}