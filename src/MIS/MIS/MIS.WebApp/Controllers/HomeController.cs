namespace MIS.WebApp.Controllers
{
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;

    using Models;

    using ViewModels.Input.Home;

    public class HomeController : BaseController
    {
        private const string Email = "mihogv@gmail.com";
        private const string Template = "<p><b>{0}</b>: {1}</p></br>";
        private const string SuccessfulMessage = "Thank you for contacting us. We will answer you soon.";

        private readonly IEmailSender emailSender;

        public HomeController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            this.TempData[GlobalConstants.TempDataContact] = SuccessfulMessage;
            var htmlMessage = new StringBuilder();
            htmlMessage.AppendLine(string.Format(Template, nameof(input.Name), input.Name));
            htmlMessage.AppendLine(string.Format(Template, nameof(input.Email), input.Email));
            htmlMessage.AppendLine(string.Format(Template, nameof(input.Subject), input.Subject));
            htmlMessage.AppendLine(string.Format(Template, nameof(input.Description), input.Description));

            await this.emailSender.SendEmailAsync(Email, input.Subject, htmlMessage.ToString());

            return this.RedirectToAction(nameof(this.Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
