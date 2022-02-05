using Newtonsoft.Json;

namespace PayloadTranslator.Handlers.Ranch_Systems.Entities
{
    public class RSMeasurement
    {
        [JsonProperty("rsuid")]
        public string Rsuid { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("subport")]
        public int Subport { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("raw")]
        public int Raw { get; set; }

        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
