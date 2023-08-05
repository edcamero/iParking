using iParking.DataAccess.DataServices.CardServices;
using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;

namespace iParking.Application.Services.CreditCard
{
    public class CreditCardServices: ICreditCardServices
    {
        private readonly ICardDataServices _cardDataServices;

        public CreditCardServices(ICardDataServices cardDataServices)
        {
            _cardDataServices = cardDataServices;
        }

        public async Task<ActionResponse> CreatedCreditCard(CreditCardInput creditCardNew)
        {
            var response = new ActionResponseSession();
            if (creditCardNew != null)
            {
                var creditCard = await _cardDataServices.GetCardAsync(creditCardNew.NumeroTarjeta, creditCardNew.CvTarjeta, creditCardNew.Tipo);

                if (creditCard != null)
                {
                    response.Message = creditCard.Estado == 0 ? "Tarjeta No esta habilitada" : "Ya existe! *Tarjeta";
                    response.Code = 409;

                    return response;
                }

                var CreditCardId = await _cardDataServices.CreatedCreditCard(creditCardNew);

                if (CreditCardId > 0)
                {
                    response.Status = true;
                    response.Code = 201;
                    response.Id = CreditCardId;

                }
                else
                {
                    response.Message = "Error tratando de crear la tarjeta";
                }

                return response;

            }
            else
            {
                response.Message = "error en los datos enviados";
                response.Code = 404;

                return response;
            }
        }

        public async Task<ActionResponse> DeleteCreditCard(CreditCardInput creditCardRequest)
        {
            var response = new ActionResponseSession();
            if (creditCardRequest != null)

            {
                var creditCard = await _cardDataServices.GetCardAsync(creditCardRequest.NumeroTarjeta, creditCardRequest.CvTarjeta, creditCardRequest.Tipo);

                if (creditCard == null)
                {
                    response.Message =  "Tarjeta No existe";
                    response.Code = 404;

                    return response;
                }

                if (creditCard.Estado == 0) {

                    response.Message = "Tarjeta No esta habilitada";
                    response.Code = 409;

                    return response;
                }

                var isDeleted = await _cardDataServices.DeletedCreditCard(creditCardRequest.IdUsuario);

                if (isDeleted)
                {
                    response.Status = true;
                    response.Code = 200;

                }
                else
                {
                    response.Message = "Error tratando de eliminar tarjeta";
                }

                return response;

            }
            else
            {
                response.Message = "error en los datos enviados";
                response.Code = 404;

                return response;
            }
        }
    }
}
