namespace MIS.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Invitation : BaseModel<string>
    {
        [Required]
        public string UserId { get; set; }
        public MISUser User { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}