using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;

namespace iParking.Application.ServicesExternal
{
    public interface IPayExtenalService
    {
        Task<ResponsePayDTO> MakePaymentPost(RequestPayExternal paymentData);
        Task<List<FormaPago>> GetPaymentMethods();
        Task<ResponsePayDTO> GetPayment(string orderId);
    }
}
