using System.Data;
using Dapper;
using iParking.Domain.Entities.MultiTenant;
using iParking.Domain.Shared;
using iParking.DataAccess.Repositories.Base;
using CompanyEntity = iParking.Domain.Entities.MultiTenant.Company;

namespace iParking.DataAccess.Repositories.Companies
{
    /// <summary>
    /// Implementación del repositorio de empresas usando Dapper.
    /// Aplica principios de Repository Pattern y parametrización SQL para seguridad.
    /// </summary>
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public CompanyRepository(IDbConnection connection) : base(connection) { }

        public async Task<Result<Company>> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM Companies 
                                 WHERE Id = @Id AND IsActive = 1";

            try
            {
                var company = await ExecuteQueryFirstOrDefaultAsync<Company>(sql, new { Id = id });
                return company != null 
                    ? Result<Company>.Success(company) 
                    : Result<Company>.Failure("Empresa no encontrada");
            }
            catch (Exception ex)
            {
                return Result<Company>.Failure($"Error al obtener empresa: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<Company>>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM Companies 
                                 WHERE IsActive = 1 
                                 ORDER BY BusinessName";

            try
            {
                var companies = await ExecuteQueryAsync<Company>(sql);
                return Result<IEnumerable<CompanyEntity>>.Success(companies);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Company>>.Failure($"Error al listar empresas: {ex.Message}");
            }
        }

        public async Task<Result<Company>> CreateAsync(Company company)
        {
            const string sql = @"INSERT INTO Companies 
                                (BusinessName, TaxId, LogoUrl, ContactEmail, ContactPhone, Address, City, 
                                 SubscriptionStartDate, SubscriptionEndDate, MaxParkingLots, MaxUsers, CreatedDate, IsActive)
                                 VALUES (@BusinessName, @TaxId, @LogoUrl, @ContactEmail, @ContactPhone, @Address, @City,
                                         @SubscriptionStartDate, @SubscriptionEndDate, @MaxParkingLots, @MaxUsers, GETDATE(), 1);
                                 SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var id = await ExecuteScalarAsync<int>(sql, company);
                company.Id = id;
                return Result<Company>.Success(company);
            }
            catch (Exception ex)
            {
                return Result<Company>.Failure($"Error al crear empresa: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(Company company)
        {
            const string sql = @"UPDATE Companies SET
                                BusinessName = @BusinessName,
                                TaxId = @TaxId,
                                LogoUrl = @LogoUrl,
                                ContactEmail = @ContactEmail,
                                ContactPhone = @ContactPhone,
                                Address = @Address,
                                City = @City,
                                SubscriptionEndDate = @SubscriptionEndDate,
                                MaxParkingLots = @MaxParkingLots,
                                MaxUsers = @MaxUsers,
                                ModifiedDate = GETDATE()
                                WHERE Id = @Id";

            try
            {
                var affected = await ExecuteNonQueryAsync(sql, company);
                return Result<bool>.Success(affected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al actualizar empresa: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            const string sql = "UPDATE Companies SET IsActive = 0, ModifiedDate = GETDATE() WHERE Id = @Id";

            try
            {
                var affected = await ExecuteNonQueryAsync(sql, new { Id = id });
                return Result<bool>.Success(affected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar empresa: {ex.Message}");
            }
        }

        public async Task<Result<Company>> GetByTaxIdAsync(string taxId)
        {
            const string sql = "SELECT * FROM Companies WHERE TaxId = @TaxId AND IsActive = 1";

            try
            {
                var company = await ExecuteQueryFirstOrDefaultAsync<Company>(sql, new { TaxId = taxId });
                return company != null 
                    ? Result<Company>.Success(company) 
                    : Result<Company>.Failure("Empresa no encontrada con ese NIT/RUT");
            }
            catch (Exception ex)
            {
                return Result<Company>.Failure($"Error al buscar empresa por NIT/RUT: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetActiveCompaniesCountAsync()
        {
            const string sql = "SELECT COUNT(*) FROM Companies WHERE IsActive = 1";

            try
            {
                var count = await ExecuteScalarAsync<int>(sql);
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al contar empresas: {ex.Message}");
            }
        }
    }
}
