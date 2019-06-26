namespace MIS.ViewModels.Input.Company
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCompanyInputModel
    {
        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Address { get; set; }
    }
}