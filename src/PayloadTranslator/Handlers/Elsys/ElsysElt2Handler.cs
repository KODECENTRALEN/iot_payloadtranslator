using System;
using Helpers;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.ELT2, "dtmi:iotplatform:elsysElt221l;1", "elt2")]
    public class ElsysElt2Handler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                var temperature = result.Temperature.Value;
                var temperatureRoad = result.ExternalTemperature1.Value;

                var bat = request.Battery * 100 / 254;
                var battery = Math.Round((double)bat, 2);

                var humidity = result.Humidity.Value;
                var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);

                response.Measurements.Add(MeasumrentType.temperature_road_c.ToString(), temperatureRoad);
                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(ElsysElt2Handler)} failed", ex);
            }

            return response;
        }
    }
}
