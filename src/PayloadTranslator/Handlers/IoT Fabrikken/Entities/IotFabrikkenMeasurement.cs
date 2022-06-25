using System;
using Newtonsoft.Json;

namespace Handlers.IotFabrikken.Entities
{
    public class IotFabrikkenMeasurement
    {
        [JsonProperty("co2")]
        public double Co2 { get; set; }

        [JsonProperty("humidity")]
        public double Humdity { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("light_colour")]
        public long LightColour { get; set; }

        [JsonProperty("light_level")]
        public long LightLevel { get; set; }

        [JsonProperty("occupancy")]
        public long Occupancy { get; set; }

        [JsonProperty("rssi")]
        public long Rssi { get; set; }

        [JsonProperty("sound")]
        public double Sound { get; set; }

        [JsonProperty("sound_high")]
        public double SoundHigh { get; set; }

        [JsonProperty("sound_low")]
        public double SoundLow { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("tstamp")]
        public DateTime Tstamp { get; set; }

        [JsonProperty("voc")]
        public double Voc { get; set; }

        [JsonProperty("voltage")]
        public long Voltage { get; set; }
    }
}