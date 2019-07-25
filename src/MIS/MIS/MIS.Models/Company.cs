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
        }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 4)]
        public string Address { get; set; }


        public ICollection<WareHouse> WareHouses { get; set; }

        public ICollection<MISUser> Employees { get; set; }

        public ICollection<Report> Reports { get; set; }

        public ICollection<Receipt> Receipts { get; set; }

        public ICollection<Invitation> Invitations { get; set; }
    }
}