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
            Data = request.Data;
            Measurements = new Dictionary<string, object>();
        }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

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
