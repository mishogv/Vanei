namespace MIS.ViewModels.Input.Company
{
    using System.ComponentModel.DataAnnotations;

    using Services.Mapping;

    using Services.Models;

    public class EditCompanyInputModel : IMapFrom<CompanyServiceModel>
    {
        public int Id { get; set; }


        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Address { get; set; }
    }
}