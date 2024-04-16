using iParking.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace iParking.Domain.ExternalServices.RequestEntities
{
    public partial class RequestGenerateOrder
    {
        [JsonProperty("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; } = new User();

        [JsonProperty("amount")]
        public Amount Amount { get; set; } = new Amount();

        [JsonProperty("methods")]
        public List<string> Methods { get; set; } = new List<string>();

        [JsonProperty("items")]
        public List<Item> Items { get; set; } = new List<Item>();

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("delivery_type")]
        public string DeliveryType { get; set; }

        [JsonProperty("generate_token")]
        public bool GenerateToken { get; set; }

        [JsonProperty("customs")]
        public List<Custom> Customs { get; set; } = new List<Custom>();


        [JsonProperty("urls")]
        public Urls Urls { get; set; } = new Urls();

        [JsonProperty("webhooks")]
        public Webhooks Webhooks { get; set; } = new Webhooks();
    }

    public partial class Amount
    {
        [JsonProperty("currency")]
        public string Currency { get; set; } = "CLP";

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; } = new Details();
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

    public partial class Urls
    {
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }
    }

    public partial class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("rut")]
        public string Rut { get; set; }

        [JsonProperty("phone")]
        public String Phone { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("address_line")]
        public string AddressLine { get; set; }

        [JsonProperty("address_city")]
        public string AddressCity { get; set; }

        [JsonProperty("address_state")]
        public string AddressState { get; set; }
    }

    public partial class Webhooks
    {
        [JsonProperty("webhook_validation")]
        public string WebhookValidation { get; set; } = "https://www.grupoayma.cl/conf.php?webhook=validation";

        [JsonProperty("webhook_confirm")]
        public string WebhookConfirm { get; set; } = "https://www.grupoayma.cl/conf.php?webhook=confirm";

        [JsonProperty("webhook_reject")]
        public string WebhookReject { get; set; } = "https://www.grupoayma.cl/conf.php?webhook=reject";
    }

    public partial class Custom
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}


