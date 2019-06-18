namespace MIS.WebApp.Areas.Administrator.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class SystemProductCreateInputModel
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