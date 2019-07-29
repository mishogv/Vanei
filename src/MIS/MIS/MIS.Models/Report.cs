namespace MIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Report : BaseModel<string>
    {
        public Report()
        {
            this.ReceiptReports = new HashSet<ReceiptReport>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public virtual ICollection<ReceiptReport> ReceiptReports { get; set; }
    }
}