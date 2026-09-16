using System.Data;
using iParking.Domain.Entities.MultiTenant;
using iParking.Domain.Shared;

namespace iParking.DataAccess.Repositories.Company
{
    /// <summary>
    /// Interfaz para el repositorio de empresas.
    /// Sigue el principio de Interface Segregation (SOLID).
    /// </summary>
    public interface ICompanyRepository
    {
        Task<Result<Company>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Company>>> GetAllAsync();
        Task<Result<Company>> CreateAsync(Company company);
        Task<Result<bool>> UpdateAsync(Company company);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<Company>> GetByTaxIdAsync(string taxId);
        Task<Result<int>> GetActiveCompaniesCountAsync();
    }
}
