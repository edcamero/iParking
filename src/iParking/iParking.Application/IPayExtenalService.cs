using iParking.Domain.Entities;

namespace iParking.Application
{
    public  interface IPayExtenalService
    {
        Task<ResponsePayDTO> MakePaymentPost(RequestPayExternal paymentData);
    }
}
