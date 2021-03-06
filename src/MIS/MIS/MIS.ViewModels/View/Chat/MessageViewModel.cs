﻿namespace MIS.ViewModels.View.Chat
{
    using Services.Mapping;
    using Services.Models;

    public class MessageViewModel : IMapFrom<MessageServiceModel>
    {
        public string Username { get; set; }

        public string Text { get; set; }
    }
}