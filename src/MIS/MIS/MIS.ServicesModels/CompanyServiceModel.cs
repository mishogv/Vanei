namespace MIS.Services.Models
{
    using System.Collections.Generic;

    using MIS.Models;

    using Mapping;

    public class CompanyServiceModel : IMapFrom<Company>
    {
        public CompanyServiceModel()
        {
            this.Employees = new HashSet<MISUser>();
            this.Reports = new HashSet<Report>();
            this.Receipts = new HashSet<Receipt>();
            this.WareHouses = new HashSet<WareHouse>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<MISUser> Employees { get; set; }

        public virtual ICollection<WareHouse> WareHouses { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}