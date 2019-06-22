namespace MIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Report : BaseModel<int>
    {
        public Report()
        {
            this.Receipts = new HashSet<Receipt>();
        }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}