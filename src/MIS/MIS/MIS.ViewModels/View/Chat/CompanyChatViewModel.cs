namespace MIS.ViewModels.View.Chat
{
    using System.Collections.Generic;

    public class CompanyChatViewModel
    {
        public CompanyChatViewModel()
        {
            this.Messages = new List<ChatHubMessageViewModel>();
        }

        public string Id { get; set; }

        public IEnumerable<ChatHubMessageViewModel> Messages { get; set; }
    }
}