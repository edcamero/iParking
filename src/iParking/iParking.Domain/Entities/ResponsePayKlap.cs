using Newtonsoft.Json;
using System.Net;

namespace iParking.Domain.Entities
{
    public  class ResponsePayKlap
    {
        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("redirect_url")]
        public Uri RedirectUrl { get; set; }

        [JsonProperty("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("ship_to")]
        public object ShipTo { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("methods")]
        public string[] Methods { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("customs")]
        public Custom[] Customs { get; set; }

        [JsonProperty("urls")]
        public Urls Urls { get; set; }

        [JsonProperty("webhooks")]
        public Webhooks Webhooks { get; set; }

        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("payment_details")]
        public object[] PaymentDetails { get; set; }

        [JsonProperty("selected_method")]
        public SelectedMethod SelectedMethod { get; set; }

        [JsonProperty("generate_token")]
        public bool GenerateToken { get; set; }
    }

    public partial class Amount
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }
    }

    public partial class Details
    {
        [JsonProperty("subtotal")]
        public long Subtotal { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("tax")]
        public long Tax { get; set; }
    }

    public partial class Custom
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("unit_price")]
        public long UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }

    public partial class Merchant
    {
        [JsonProperty("rut")]
        public long Rut { get; set; }

        [JsonProperty("business_name")]
        public string BusinessName { get; set; }

        [JsonProperty("brand_name")]
        public string BrandName { get; set; }
    }

    public partial class SelectedMethod
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Urls
    {
        [JsonProperty("return_url")]
        public Uri ReturnUrl { get; set; }

        [JsonProperty("cancel_url")]
        public Uri CancelUrl { get; set; }

        [JsonProperty("logo_url")]
        public object LogoUrl { get; set; }
    }

    public partial class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("rut")]
        public long Rut { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public long Phone { get; set; }

        [JsonProperty("address_line")]
        public string AddressLine { get; set; }

        [JsonProperty("address_city")]
        public string AddressCity { get; set; }

        [JsonProperty("address_state")]
        public string AddressState { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }

    public partial class Webhooks
    {
        [JsonProperty("webhook_validation")]
        public Uri WebhookValidation { get; set; }

        [JsonProperty("webhook_confirm")]
        public Uri WebhookConfirm { get; set; }

        [JsonProperty("webhook_reject")]
        public Uri WebhookReject { get; set; }
    }
}
