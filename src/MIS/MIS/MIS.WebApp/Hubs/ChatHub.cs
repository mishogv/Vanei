namespace MIS.WebApp.Hubs
{
    using System.Threading.Tasks;

    using Common;

    using Ganss.XSS;

    using Microsoft.AspNetCore.SignalR;

    using Services;
    using Services.Mapping;

    using ViewModels.View.Chat;

    public class ChatHub : Hub
    {
        private const string SignalRMethodName = "NewMessage";
        private const string MessageJoinGroupTemplate = "{0} has joined the group {1}";
        private const string MessageLeftGroupTemplate = "{0} has left the group {1}";

        private readonly IHtmlSanitizer sanitizer;
        private readonly IMessageService messageService;

        public ChatHub(IHtmlSanitizer sanitizer,
            IMessageService messageService)
        {
            this.sanitizer = sanitizer;
            this.messageService = messageService;
        }

        public async Task AddToGroup(string companyId)
        {
            var username = this.Context.User.Identity.Name;
            var message = await this.messageService.CreateAsync(companyId, username, MessageJoinGroupTemplate, true);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, message.Company.Name);
            message.Username = GlobalConstants.System;

            await this.Clients.Group(message.Company.Name)
                         .SendAsync(SignalRMethodName, message.MapTo<MessageViewModel>());
        }

        public async Task RemoveFromGroup(string companyId)
        {
            var username = this.Context.User.Identity.Name;
            var message = await this.messageService.CreateAsync(companyId, username, MessageLeftGroupTemplate, true);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, message.Company.Name);

            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, message.Company.Name);
            message.Username = GlobalConstants.System;

            await this.Clients.Group(message.Company.Name)
                      .SendAsync(SignalRMethodName, message.MapTo<MessageViewModel>());
        }

        public async Task Send(string companyId, string message)
        {
            var username = this.Context.User.Identity.Name;
            var sanitizedMessage = this.sanitizer.Sanitize(message);

            var generatedMessage = await this.messageService.CreateAsync(companyId, username, sanitizedMessage, false);

            await this.Clients.Group(generatedMessage.Company.Name)
                      .SendAsync(SignalRMethodName, generatedMessage.MapTo<MessageViewModel>());
        }
    }
}