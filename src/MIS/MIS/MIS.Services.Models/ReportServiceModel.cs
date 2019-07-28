namespace MIS.Services.Models
{
    using System;
    using System.Collections.Generic;

    using MIS.Models;

    using Models;

    using Services.Mapping;

    public class ReportServiceModel : IMapFrom<Report>
    {
        public ReportServiceModel()
        {
            this.ReceiptReports = new HashSet<ReceiptReport>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public virtual ICollection<ReceiptReport> ReceiptReports { get; set; }
    }
}