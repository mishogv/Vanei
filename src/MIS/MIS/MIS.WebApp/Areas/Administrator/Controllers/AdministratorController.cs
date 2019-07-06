namespace MIS.WebApp.Areas.Administrator.Controllers
{
    using Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using WebApp.Controllers;

    [Area(GlobalConstants.AdministratorAreaRole)]
    [Authorize(Roles = GlobalConstants.AdministratorAreaRole)]
    public class AdministratorController : BaseController
    {
        public AdministratorController()
        {
        }
    }
}