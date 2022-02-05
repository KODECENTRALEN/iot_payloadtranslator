using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.TSBIN)]
    public class TsbinHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                var messageCode = hexBytes[0].StringToByteArray()[0].HighBit();

                //// documentation states that data frame is hex 0x0D and hex 0x0F
                if (messageCode == 15 || messageCode == 13)
                {
                    //// documentation states that the third byte's upper bits are the battery level indicator
                    var batteryLevel = binaryString.Substring(8, 4).FromBinaryToDecimal();
                    var batteryPercentage = BatteryLevelToPercentage(batteryLevel);
                    //// documentation states that the fourth byte is the distance
                    var distance = hexBytes[3].FromHexToDecimal();

                    response.Measurements.Add(MeasumrentType.battery_pct.ToString(), batteryPercentage);
                    response.Measurements.Add(MeasumrentType.distance_cm.ToString(), distance);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TsBinMeasurements failed", ex);
            }

            return response;
        }

        private double BatteryLevelToPercentage(double batteryLevel)
        {
            //// From documentation: It goes from 0 ((Battery very good) to 3 (Battery level poor).
            return batteryLevel switch
            {
                0 => 100,
                1 => 70,
                2 => 40,
                3 => 10,
                _ => 0,
            };
        }
    }
}
