using System;
using Bluefragments.Utilities.Extensions;
using Newtonsoft.Json;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers.IotFabrikken.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers.IotFabrikken
{
    [Sensor(DeviceTypes.IOTFABRIKKEN, "dtmi:iotplatform:iotFabrikken3fk;1", "xxxxxxxxxx")]
    public class IotFabrikkenHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = JsonConvert.DeserializeObject<IotFabrikkenMeasurement>(request.Data);

                var timeStamp = result.Tstamp.ToEpochTimeSeconds();

                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), result.Temperature);
                response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), result.Humdity);
                response.Measurements.Add(MeasumrentType.sound_peak_db.ToString(), result.SoundHigh);
                response.Measurements.Add(MeasumrentType.sound_current_db.ToString(), result.Sound);
                response.Measurements.Add(MeasumrentType.sound_low_db.ToString(), result.SoundLow);
                response.Measurements.Add(MeasumrentType.co2_ppm.ToString(), result.Co2);
                response.Measurements.Add(MeasumrentType.light_color.ToString(), result.LightColour);
                response.Measurements.Add(MeasumrentType.light_level.ToString(), result.LightLevel);
                response.Measurements.Add(MeasumrentType.occupancy.ToString(), result.Occupancy);
                response.Measurements.Add(MeasumrentType.rssi_dbm.ToString(), result.Rssi);
                response.Measurements.Add(MeasumrentType.voc_ppb.ToString(), result.Voc);
                response.Measurements.Add(MeasumrentType.voltage_v.ToString(), result.Voltage);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(IotFabrikkenHandler)} failed", ex);
            }

            return response;
        }
    }
}
