using System;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers.Sensative.Helpers;
using Data.Enums;

namespace PayloadTranslator.Handlers
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

                response.Measurements.Add(MeasurementType.count_acc.ToString(), doorCount);
                response.Measurements.Add(MeasurementType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasurementType.battery_pct.ToString(), battery);
            }
            catch (Exception ex)
            {
                throw new Exception("SensativeStrips Measurements failed", ex);
            }

            return response;
        }
    }
}
