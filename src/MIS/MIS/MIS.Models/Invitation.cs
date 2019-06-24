namespace MIS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Invitation : BaseModel<int>
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }
    }
}