namespace Vanei.WebApp.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CompanyImportViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Address { get; set; }
    }
}