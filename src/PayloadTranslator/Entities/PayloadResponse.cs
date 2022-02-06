using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayloadTranslator.Entities
{
    public class PayloadResponse
    {
        public PayloadResponse(PayloadRequest request)
        {
            Time = request.Time;
            DeviceType = request.DeviceType;
            DeviceId = request.DeviceId;
            ModelId = request.ModelId;
            Data = request.Data;
            Measurements = new Dictionary<string, object>();
        }

        public PayloadResponse(string error)
        {
            Error = error;
        }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("modelId")]
        public string ModelId { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("measurements")]
        public IDictionary<string, object> Measurements { get; set; }
    }
}
