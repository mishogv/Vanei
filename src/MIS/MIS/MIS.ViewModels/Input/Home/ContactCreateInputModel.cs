namespace MIS.ViewModels.Input.Home
{
    using System.ComponentModel.DataAnnotations;

    public class ContactCreateInputModel
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }
    }
}