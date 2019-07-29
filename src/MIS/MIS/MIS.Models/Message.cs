namespace MIS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message : BaseModel<string>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime AddedOn { get; set; }


        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}