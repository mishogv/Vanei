namespace MIS.ServicesModels
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class ReportServiceModel
    {
        public ReportServiceModel()
        {
            this.Receipts = new HashSet<Receipt>();
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}