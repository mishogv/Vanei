namespace MIS.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    using ServicesModels;

    public interface ICompanyService
    {
        Task<CompanyServiceModel> CreateAsync(string name, string address, string userId);

        Task<CompanyServiceModel> CreateAsync(string name, string address);

        Task<CompanyServiceModel> AddToCompanyAsync(string name, string username);
    }
}