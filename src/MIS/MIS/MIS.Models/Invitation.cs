namespace MIS.Models
{
    public class Invitation : BaseModel<int>
    {
        public string UserId { get; set; }
        public MISUser User { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}