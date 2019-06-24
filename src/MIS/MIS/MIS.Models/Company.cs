namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Company : BaseModel<int>
    {
        public Company()
        {
            this.Users = new HashSet<MISUser>();
            this.Invitations = new HashSet<Invitation>();
            this.Reports = new HashSet<Report>();
        }

        [Required]
        [StringLength(GlobalConstants.CompanyNameMaximumLength, MinimumLength = GlobalConstants.CompanyNameMinimumLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(GlobalConstants.CompanyAddressMaximumLength, MinimumLength = GlobalConstants.CompanyAddressMinimumLength)]
        public string Address { get; set; }

        public string OwnerId { get; set; }
        public virtual MISUser Owner { get; set; }

        public int? WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }

        public ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<MISUser> Users { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}