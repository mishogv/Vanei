namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Enums;

    using Microsoft.AspNetCore.Identity;

    // Add profile data for application users by adding properties to the MISUser class
    public class MISUser : IdentityUser
    {
        public MISUser()
        {
            this.SystemProducts = new HashSet<SystemProduct>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        public CompanyRole Role { get; set; } = CompanyRole.Employee;

        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public ICollection<SystemProduct> SystemProducts { get; set; }
    }
}
