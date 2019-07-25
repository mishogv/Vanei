namespace MIS.ViewModels.View.Invitation
{
    using Services.Mapping;

    using ServicesModels;

    public class InvitationIndexDetailsViewModel : IMapFrom<InvitationServiceModel>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string CompanyName { get; set; }
    }
}