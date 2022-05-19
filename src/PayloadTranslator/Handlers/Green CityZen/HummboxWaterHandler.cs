using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.HUMMBOXWATER)]
    public class HummboxWaterHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                var messageCode = hexBytes[0].FromHexToDecimal();

                //// documentation states that payload is only relevant for hex 10 and hex12
                if (messageCode == 16 || messageCode == 18)
                {
                    //// doc says do little Endian. aka reverse order
                    var distance = (hexBytes[2] + hexBytes[1]).FromHexToDouble();
                    //// new device HACK
                    if (request.DeviceId != "202618")
                    {
                        //// convert from mm to cm
                        distance = distance / 10;
                    }

                    var battery = hexBytes[4].FromHexToDecimal();

                    response.Measurements.Add(MeasurementType.distance_cm.ToString(), distance);
                    response.Measurements.Add(MeasurementType.battery_pct.ToString(), battery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HummBoxWaterMeasurements failed", ex);
            }

            return response;
        }
    }
}
