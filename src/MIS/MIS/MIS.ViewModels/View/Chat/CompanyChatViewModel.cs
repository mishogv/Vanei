namespace MIS.ViewModels.View.Chat
{
    using System.Collections.Generic;

    public class CompanyChatViewModel
    {
        public CompanyChatViewModel()
        {
            this.Messages = new List<MessageViewModel>();
        }

        public string Id { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}