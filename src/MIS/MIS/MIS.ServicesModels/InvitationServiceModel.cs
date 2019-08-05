namespace MIS.Services.Models
{
    using MIS.Models;

    using Mapping;

    public class InvitationServiceModel : IMapFrom<Invitation>
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public MISUser User { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }
    }
}