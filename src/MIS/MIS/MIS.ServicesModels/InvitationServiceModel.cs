namespace MIS.ServicesModels
{
    using Models;

    public class InvitationServiceModel
    {
        public string From { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string UserId { get; set; }
        public virtual MISUser User { get; set; }
    }
}