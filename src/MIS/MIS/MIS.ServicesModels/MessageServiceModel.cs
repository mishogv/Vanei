namespace MIS.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    using Mapping;

    using MIS.Models;

    public class MessageServiceModel : IMapFrom<Message>
    {
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}