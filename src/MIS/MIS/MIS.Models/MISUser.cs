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
            this.Receipts = new HashSet<Receipt>();
            this.Reports = new HashSet<Report>();
            this.Invitations = new HashSet<Invitation>();
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

        public virtual ICollection<SystemProduct> SystemProducts { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
