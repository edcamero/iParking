using iParking.Domain.Entities.Payment;

namespace iParking.DataAccess.DataServices.CardServices
{
    public  interface ICardDataServices
    {
        Task<CreditCard?> GetCardAsync(string numero_tarjeta, string cvv, string tipo);
        Task<int> CreatedCreditCard(CreditCardInput creditCard);
        Task<bool> DeletedCreditCard(int creditCardId);
    }
}
