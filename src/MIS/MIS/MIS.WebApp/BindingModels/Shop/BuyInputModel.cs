namespace MIS.WebApp.BindingModels.Shop
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class BuyInputModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(GlobalConstants.CompanyNameMaximumLength, MinimumLength = GlobalConstants.CompanyNameMinimumLength)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(GlobalConstants.CompanyAddressMaximumLength, MinimumLength = GlobalConstants.CompanyAddressMinimumLength)]
        public string Address { get; set; }
    }
}