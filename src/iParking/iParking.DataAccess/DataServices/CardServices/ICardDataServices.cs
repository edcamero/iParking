using iParking.Domain.Entities.Payment;

namespace iParking.DataAccess.DataServices.CardServices
{
    public  interface ICardDataServices
    {
        Task<CreditCard?> GetCardAsync(string numero_tarjeta, string cvv, string tipo);
        Task<CreditCard?> GetCardAsync(int userId, int creditCardId);
        Task<int> CreatedCreditCard(CreditCardInput creditCard);
        Task<bool> DeletedCreditCard(int userId, int creditCardId);
        Task<bool> UpdatedCreditCardDefault(int userId, int creditCardId);
    }
}
