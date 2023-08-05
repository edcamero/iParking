using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;

namespace iParking.Application.Services.CreditCard
{
    public interface ICreditCardServices
    {
        Task<ActionResponse> CreatedCreditCard(CreditCardInput creditCardNew);
        Task<ActionResponse> DeleteCreditCard(int userId, int creditCardId);
        Task<ActionResponseSession> UpdatedCreditCardDefault(int userId, int creditCardId);
    }
}
