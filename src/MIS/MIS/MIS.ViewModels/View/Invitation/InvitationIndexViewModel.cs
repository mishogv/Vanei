namespace MIS.ViewModels.View.Invitation
{
    using System.Collections.Generic;

    public class InvitationIndexViewModel
    {
        public InvitationIndexViewModel()
        {
            this.Invitations = new List<InvitationIndexDetailsViewModel>();
        }

        public IList<InvitationIndexDetailsViewModel> Invitations { get; set; }
    }
}