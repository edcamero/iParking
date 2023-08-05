using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;

namespace iParking.Application
{
    public  interface IPayExtenalService
    {
        Task<ResponsePayDTO> MakePaymentPost(RequestPayExternal paymentData);
        Task<List<FormaPago>> GetPaymentMethods();
    }
}
