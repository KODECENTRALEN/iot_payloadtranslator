using Newtonsoft.Json;

namespace Entities;

public class PayloadRequest
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("battery")]
    public int Battery { get; set; }

    [JsonProperty("deviceType")]
    public string DeviceType { get; set; }
}
