namespace MIS.ViewModels.Input.Category
{
    using System.ComponentModel.DataAnnotations;

    using Models;

    using Services.Mapping;

    public class CategoryCreateWareHouseInputModel : IMapFrom<Category>
    {
        private const int MinLength = 2;
        private const int MaxLength = 24;

        public string Id { get; set; }


        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }
    }
}