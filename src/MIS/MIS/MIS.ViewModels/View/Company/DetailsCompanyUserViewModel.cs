namespace MIS.ViewModels.View.Company
{
    using Models;

    using Services.Mapping;

    public class DetailsCompanyUserViewModel : IMapFrom<MISUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}