using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;

namespace iParking.Application.Services.CreditCard
{
    public interface ICreditCardServices
    {
        Task<ActionResponse> CreatedCreditCard(CreditCardInput creditCardNew);
        Task<ActionResponse> DeleteCreditCard(CreditCardInput creditCardRequest);
    }
}
