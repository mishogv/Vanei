namespace Vanei.WebApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company : BaseModel<int>
    {
        public Company()
        {
            this.Users = new HashSet<WebAppUser>();
        }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Address { get; set; }

        public string OwnerId { get; set; }
        public virtual WebAppUser Owner { get; set; }

        public int? WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }

        public virtual ICollection<WebAppUser> Users { get; set; }
    }
}