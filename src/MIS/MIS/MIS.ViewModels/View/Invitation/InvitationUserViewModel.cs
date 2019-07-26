namespace MIS.ViewModels.View.Invitation
{
    using System.Collections.Generic;

    using Models;

    using Services.Mapping;

    public class InvitationUserViewModel : IMapFrom<MISUser>
    {
        public InvitationUserViewModel()
        {
            this.Invitations = new List<Invitation>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string CompanyName { get; set; }

        public int? CompanyId { get; set; }

        public bool IsAvailableForInvite { get; set; }

        public IEnumerable<Invitation> Invitations { get; set; }
    }
}