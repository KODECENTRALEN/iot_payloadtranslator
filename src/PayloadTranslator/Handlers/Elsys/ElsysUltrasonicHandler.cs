using Enums;
using Attributes;
using Entities;
using Enums;

namespace Handlers
{
    [Sensor(DeviceTypes.ULTRASONIC, "dtmi:generic:generic;1", "ULTRASONIC")]
    public class ElsysUltrasonicHandler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.ULTRASONIC;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                var distance = result.Distance.Value / 10;
                var temperature = result.Temperature.Value;
                var humidity = result.Humidity.Value;

                var bat = request.Battery * 100 / 254;
                var battery = Math.Round((double)bat, 2);

                response.Measurements.Add(MeasumrentType.distance_cm.ToString(), distance);
                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
            }
            catch (Exception ex)
            {
                throw new Exception("ElsysUltrasonicMeasurements failed", ex);
            }

            return response;
        }
    }
}
