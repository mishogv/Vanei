namespace MIS.ViewModels.View.Invitation
{
    using System.Collections.Generic;

    using Models;

    using Services.Mapping;
    using Services.Models;

    public class InvitationUserViewModel : IMapFrom<MISUserServiceModel>
    {
        public InvitationUserViewModel()
        {
            this.Invitations = new List<Invitation>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string CompanyName { get; set; }

        public string CompanyId { get; set; }

        public bool IsAvailableForInvite { get; set; }

        public IEnumerable<Invitation> Invitations { get; set; }
    }
}