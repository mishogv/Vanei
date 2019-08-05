namespace MIS.ViewModels.Input.Company
{
    using System.ComponentModel.DataAnnotations;

    using Services.Mapping;

    using Services.Models;

    public class CompanyEditInputModel : IMapFrom<CompanyServiceModel>
    {
        private const int MinLength = 4;
        private const int MaxLength = 40;

        public string Id { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Address { get; set; }
    }
}