namespace MIS.Services
{
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    public interface ICompanyService
    {
        Task<CompanyServiceModel> CreateAsync(string name, string address, string ownerId);

        Task<CompanyServiceModel> GetByUserAsync(MISUser user);
    }
}