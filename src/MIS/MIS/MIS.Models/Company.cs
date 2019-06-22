namespace MIS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company : BaseModel<int>
    {
        public Company()
        {
            this.Users = new HashSet<MISUser>();
            this.Reports = new HashSet<Report>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Address { get; set; }

        public string OwnerId { get; set; }
        public virtual MISUser Owner { get; set; }

        public int? WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }

        public virtual ICollection<MISUser> Users { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}