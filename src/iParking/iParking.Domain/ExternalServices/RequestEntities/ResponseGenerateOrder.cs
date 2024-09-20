using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iParking.Domain.ExternalServices.RequestEntities
{
    public partial class ResponseGenerateOrder
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
        public List<string> Methods { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("customs")]
        public List<Custom> Customs { get; set; }

        [JsonProperty("urls")]
        public Urls Urls { get; set; }

        [JsonProperty("webhooks")]
        public Webhooks Webhooks { get; set; }

        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("payment_details")]
        public List<object> PaymentDetails { get; set; }

        [JsonProperty("selected_method")]
        public SelectedMethod SelectedMethod { get; set; }

        [JsonProperty("generate_token")]
        public bool GenerateToken { get; set; }
    }
    public partial class Merchant
    {
        [JsonProperty("rut")]
        public string Rut { get; set; }

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
}


