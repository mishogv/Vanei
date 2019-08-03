namespace MIS.Models
{
    using System.ComponentModel.DataAnnotations;

    using Contracts;

    public class Invitation : BaseModel<string>, IHaveCompany
    {
        [Required]
        public string UserId { get; set; }
        public MISUser User { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}