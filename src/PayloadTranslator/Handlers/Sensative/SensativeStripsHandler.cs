using Enums;
using Helpers;
using Attributes;
using Entities;
using Enums;
using Handlers.Sensative.Helpers;

namespace Handlers
{
    [Sensor(DeviceTypes.SENSATIVESTRIPS)]
    public class SensativeStripsHandler : Handler, IHandler
    {
        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.SENSATIVESTRIPS;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var bytes = HexHelper.HexToBytes(request.Data);
                var result = SensativeStripsDecoder.Decoder(bytes, 1);
                var temperature = result.Temperature;
                var bat = request.Battery * 100 / 254;
                var battery = Math.Round((double)bat, 2);
                var doorCount = result.DoorCount;

                response.Measurements.Add(MeasumrentType.count_acc.ToString(), doorCount);
                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
            }
            catch (Exception ex)
            {
                throw new Exception("SensativeStrips Measurements failed", ex);
            }

            return response;
        }
    }
}
