namespace MIS.WebApp.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;

    using Microsoft.AspNetCore.SignalR;

    using ViewModels.View.Chat;

    public class ChatHub : Hub
    {
        private readonly IHtmlSanitizer sanitizer;

        public ChatHub(IHtmlSanitizer sanitizer)
        {
            this.sanitizer = sanitizer;
        }

        public async Task AddToGroup(string groupName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);

            await this.Clients.Group(groupName)
                         .SendAsync("NewMessage", 
                             new ChatHubMessageViewModel
                             {
                                 Username = this.Context.User.Identity.Name,
                                 Text = $"{this.Context.User.Identity.Name} has joined the group."
                             });
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupName);

            await this.Clients.Group(groupName).SendAsync("NewMessage",
            new ChatHubMessageViewModel
            {
                Username = this.Context.User.Identity.Name,
                Text = $"{this.Context.User.Identity.Name} has left the group."
            });
        }

        public async Task Send(string group, string message)
        {
            var sanitizedMessage = this.sanitizer.Sanitize(message);

            await this.Clients.Group(group).SendAsync("NewMessage",
                new ChatHubMessageViewModel
                {
                    Username = this.Context.User.Identity.Name,
                    Text = sanitizedMessage,
                });
        }
    }
}