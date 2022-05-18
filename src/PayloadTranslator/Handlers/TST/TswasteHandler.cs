using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.TSWASTE)]
    public class TswasteHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                var messageCode = hexBytes[0].StringToByteArray()[0].HighBit();

                //// documentation states that data frame is hex 0xF and hex 0xD
                if (messageCode == 15 || messageCode == 13)
                {
                    //// documentation states that the second byte's upper bits are the battery level indicator
                    var batteryLevel = hexBytes[1].StringToByteArray()[0].HighBit();
                    //// documentation states that the last bit of the fourth byte plus the fith byte is the distance
                    var distance = binaryString.Substring(23, 9).FromBinaryToDecimal();

                    response.Measurements.Add(MeasurementType.battery_pct.ToString(), batteryLevel);
                    response.Measurements.Add(MeasurementType.distance_cm.ToString(), distance);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TsWasteMeasurements failed", ex);
            }

            return response;
        }
    }
}
