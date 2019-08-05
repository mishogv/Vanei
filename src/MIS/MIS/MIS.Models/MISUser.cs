namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    using Microsoft.AspNetCore.Identity;

    public class MISUser : IdentityUser, IHaveCompany
    {
        private const int MinLength = 3;
        private const int MaxLength = 30;

        public MISUser()
        {
            this.Receipts = new HashSet<Receipt>();
            this.Reports = new HashSet<Report>();
            this.Invitations = new HashSet<Invitation>();
        }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string LastName { get; set; }

        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}
