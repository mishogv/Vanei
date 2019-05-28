namespace Vanei.Data.Models.Models
{
    using System.ComponentModel.DataAnnotations;

    using Enums;

    using Microsoft.AspNetCore.Identity;

        
    public class WebAppUser : IdentityUser
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        public CompanyRole Role { get; set; }

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
