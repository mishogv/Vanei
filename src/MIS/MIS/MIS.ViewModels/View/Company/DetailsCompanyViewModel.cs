namespace MIS.ViewModels.View.Company
{
    using System.Collections.Generic;

    using Services.Mapping;

    using ServicesModels;

    public class DetailsCompanyViewModel : IMapFrom<CompanyServiceModel>
    {
        public DetailsCompanyViewModel()
        {
            this.Employees = new List<DetailsCompanyUserViewModel> ();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsOwner { get; set; }

        public IEnumerable<DetailsCompanyUserViewModel> Employees { get; set; }
    }
}