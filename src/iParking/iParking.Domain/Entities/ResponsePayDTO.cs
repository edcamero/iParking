
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace iParking.Domain.Entities
{


    public class ResponsePayDTO
    {
       public ResponsePayDTO(bool status, string info)
        {
            Status = status;
            Data = status ? new ResponseData(info, null) : new ResponseData(null, info);
        }

        [JsonProperty("estatus")]
        public bool Status { get; set; }

        [JsonProperty("datos")]
        public ResponseData Data { get; set; }
    }

    public class ResponseData
    {
        public ResponseData(string? url, string? errorMessage)
        {
            ErrorMessage = errorMessage;
            Url = url;
        }

        [JsonProperty("mensajeError")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        [JsonProperty("url")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }
    }

}

