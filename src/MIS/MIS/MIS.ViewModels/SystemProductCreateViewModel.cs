namespace MIS.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SystemProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price  { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        [Required]
        public string Description { get; set; }
    }
}