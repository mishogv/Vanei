﻿namespace MIS.ViewModels.View.Invitation
{
    using Services.Mapping;

    using Services.Models;

    public class InvitationIndexDetailsViewModel : IMapFrom<InvitationServiceModel>
    {
        public string Id { get; set; }

        public string Username { get; set; }
            
        public string CompanyName { get; set; }
    }
}