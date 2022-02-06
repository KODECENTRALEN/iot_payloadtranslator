using Newtonsoft.Json;

namespace PayloadTranslator.Entities;

public class PayloadRequest
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; }

    [JsonProperty("modelId")]
    public string ModelId { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("battery")]
    public int Battery { get; set; }

    [JsonProperty("deviceType")]
    public string DeviceType { get; set; }

    public dynamic Payload { get; set; }
}
