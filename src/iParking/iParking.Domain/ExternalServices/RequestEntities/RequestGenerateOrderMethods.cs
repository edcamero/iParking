using iParking.Domain.Entities;

namespace iParking.Domain.ExternalServices.RequestEntities
{
    public class RequestGenerateOrderMethods
    {

        public static RequestGenerateOrder CreateRequest(RequestPayExternal paymentData)
        {
            var request =  new RequestGenerateOrder();
            request.ReferenceId = paymentData.reference_id;
            request.User.Email = paymentData.email;
            request.User.Rut = paymentData.rut;
            request.User.Phone = paymentData.phone.Replace(" ", "");
            request.User.FirstName = paymentData.first_name;
            request.User.LastName = paymentData.last_name;
            request.User.AddressLine = paymentData.address_line;
            request.User.AddressCity = paymentData.address_city;
            request.User.AddressState = paymentData.address_state;
            request.Amount.Total = (long)Convert.ToDouble(paymentData.amount);
            request.Amount.Details.Subtotal = request.Amount.Total;
            request.Items.Add(new Item() { Code = paymentData.code_item, Name = paymentData.name_item , Price = request.Amount.Total, UnitPrice= request.Amount.Total, Quantity= 1 });
            request.Methods.Add("tarjetas");
            request.DeliveryType = "1";
            request.Description = "Esta es una orden en ambiente de produccion para pago con tarjeta.";
            request.GenerateToken = true;
            request.Urls.ReturnUrl = "https://www.grupoayma.cl/conf.html";
            request.Urls.CancelUrl = "https://www.grupoayma.cl/rech.html";
            request.Customs.Add(new Custom() { Key = "payments_notify_user", Value= "false" });
            request.Customs.Add(new Custom() { Key = "tarjetas_expiration_minutes", Value= "10" });

            return request;
        }
    }
}
