namespace MIS.ViewModels.View.Chat
{
    using System.Collections.Generic;

    public class CompanyChatViewModel
    {
        public CompanyChatViewModel()
        {
            this.Messages = new List<ChatHubMessageViewModel>();
        }

        public int Id { get; set; }

        public IEnumerable<ChatHubMessageViewModel> Messages { get; set; }
    }
}