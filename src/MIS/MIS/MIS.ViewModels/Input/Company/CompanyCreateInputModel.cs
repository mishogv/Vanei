namespace MIS.ViewModels.Input.Company
{
    using System.ComponentModel.DataAnnotations;

    public class CompanyCreateInputModel
    {
        private const int MinLength = 4;
        private const int MaxLength = 40;

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Address { get; set; }
    }
}