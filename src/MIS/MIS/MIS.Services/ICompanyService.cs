namespace MIS.Services
{
    using System.Threading.Tasks;

    using ServicesModels;

    public interface ICompanyService
    {
        Task<CompanyServiceModel> CreateAsync(string name, string address, string ownerId);
    }
}