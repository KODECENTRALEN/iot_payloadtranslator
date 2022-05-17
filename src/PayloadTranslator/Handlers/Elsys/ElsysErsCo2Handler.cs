using Data.Enums;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.ERSCO2, "dtmi:generic:generic;1", "ERSCO2")]
    public class ElsysErsCo2Handler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                var co2 = result.Co2.Value;
                var humidity = result.Humidity.Value;
                var lux = result.Light.Value;
                var pir = result.Motion.Value;
                var temperature = result.Temperature.Value;

                double currentVoltage = result.Vdd.Value;
                double bat = ((currentVoltage - 1500) / (3700 - 1500)) * 100;
                var battery = Math.Round(bat, 2);

                response.Measurements.Add(MeasumrentType.co2_ppm.ToString(), co2);
                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                response.Measurements.Add(MeasumrentType.lux_lumen.ToString(), lux);
                response.Measurements.Add(MeasumrentType.movement.ToString(), pir);
            }
            catch (Exception ex)
            {
                throw new Exception("ElsysErsCo2Measurements failed", ex);
            }

            return response;
        }
    }
}
