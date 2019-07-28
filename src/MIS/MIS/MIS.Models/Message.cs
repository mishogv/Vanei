namespace MIS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message : BaseModel<int>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime AddedOn { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}