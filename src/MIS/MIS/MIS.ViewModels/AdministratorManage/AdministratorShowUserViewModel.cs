namespace MIS.ViewModels.AdministratorManage
{
    using System.Collections.Generic;

    using Models;
    using Models.Enums;

    public class AdministratorShowUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CompanyRole { get; set; }

        public string Role { get; set; }

        public string CompanyName { get; set; }
    }
}