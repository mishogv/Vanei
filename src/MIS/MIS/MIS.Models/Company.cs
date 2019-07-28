namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company : BaseModel<int>
    {
        public Company()
        {
            this.Employees = new HashSet<MISUser>();
            this.Receipts = new HashSet<Receipt>();
            this.Reports = new HashSet<Report>();
            this.WareHouses = new HashSet<WareHouse>();
            this.Invitations = new HashSet<Invitation>();
            this.Messages = new HashSet<Message>();
        }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Address { get; set; }


        public virtual ICollection<WareHouse> WareHouses { get; set; }

        public virtual ICollection<MISUser> Employees { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}