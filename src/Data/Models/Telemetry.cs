using Newtonsoft.Json;

namespace Data.Models;

public class Telemetry : BaseModel
{
    [JsonProperty("deviceId")]
    public string DeviceId { get; set; }

    [JsonProperty("measurements")]
    public IDictionary<string, object> Measurements { get; set; }

    [JsonProperty("ts")]
    public DateTime Time { get; set; }
}
