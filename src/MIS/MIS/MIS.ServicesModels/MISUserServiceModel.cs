namespace MIS.Services.Models
{
    using System.Collections.Generic;

    using Mapping;

    using MIS.Models;

    public class MISUserServiceModel : IMapFrom<MISUser>
    {
        public MISUserServiceModel()
        {
            this.Receipts = new HashSet<Receipt>();
            this.Reports = new HashSet<Report>();
            this.Invitations = new HashSet<Invitation>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
    }
}