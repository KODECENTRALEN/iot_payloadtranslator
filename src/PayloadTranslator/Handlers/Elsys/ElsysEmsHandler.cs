using Data.Enums;
using Helpers;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.EMS, "dtmi:generic:generic;1", "ems")]
    public class ElsysEmsHandler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);


                if (result.ExternalTemperature1 != null)
                {
                    var temperatureRoad = result.ExternalTemperature1.Value;
                    response.Measurements.Add(MeasumrentType.temperature_road_c.ToString(), temperatureRoad);
                }

                if (result.Temperature != null)
                {
                    var temperature = result.Temperature.Value;
                    response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                }

                if (result.Humidity != null)
                {
                    var humidity = result.Humidity.Value;
                    response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                }

                if (result.Temperature != null && result.Humidity != null)
                {
                    var temperature = result.Temperature.Value;
                    var humidity = result.Humidity.Value;
                    var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);
                    response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);
                }

                if (request.Battery > 0)
                {
                    var bat = request.Battery * 100 / 254;
                    var battery = Math.Round((double)bat, 2);
                    response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(ElsysElt2Handler)} failed", ex);
            }

            return response;
        }
    }
}
