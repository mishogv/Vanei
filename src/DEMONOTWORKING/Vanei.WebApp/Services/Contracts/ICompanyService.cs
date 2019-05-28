namespace Vanei.WebApp.Services.Contracts
{
    using System.Threading.Tasks;

    using Data;

    using Vanei.Data.Models.Models;

    public interface ICompanyService
    {
        Task CreateCompanyAsync(params string[] args);
        Task<bool> DeleteCompanyAsync(WebAppUser user, string company);
        Task<bool> DeleteCompanyAsync(WebAppUser user, Company company);
    }
}