namespace MIS.ServicesModels
{
    using System.Collections.Generic;

    using Models;

    public class CompanyServiceModel
    {
        public CompanyServiceModel()
        {
            this.Users = new HashSet<MISUser>();
            this.Reports = new HashSet<Report>();
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string OwnerId { get; set; }
        public virtual MISUser Owner { get; set; }

        public int? WareHouseId { get; set; }
        public virtual WareHouse WareHouse { get; set; }

        public virtual ICollection<MISUser> Users { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}