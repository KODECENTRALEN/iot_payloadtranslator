using Data.Enums;
using Helpers;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.ELT2, "dtmi:generic:generic;1", "elt2")]
    public class ElsysElt2Handler : Handler, IHandler
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
                    response.Measurements.Add(MeasurementType.temperature_road_c.ToString(), temperatureRoad);
                }

                if (result.Temperature != null)
                {
                    var temperature = result.Temperature.Value;
                    response.Measurements.Add(MeasurementType.temperature_c.ToString(), temperature);
                }

                if (result.Humidity != null)
                {
                    var humidity = result.Humidity.Value;
                    response.Measurements.Add(MeasurementType.humidity_pct.ToString(), humidity);
                }

                if (result.Temperature != null && result.Humidity != null)
                {
                    var temperature = result.Temperature.Value;
                    var humidity = result.Humidity.Value;
                    var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);
                    response.Measurements.Add(MeasurementType.dewpoint_c.ToString(), dewpoint);
                }

                if (request.Battery > 0)
                {
                    var bat = request.Battery * 100 / 254;
                    var battery = Math.Round((double)bat, 2);
                    response.Measurements.Add(MeasurementType.battery_pct.ToString(), battery);
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
