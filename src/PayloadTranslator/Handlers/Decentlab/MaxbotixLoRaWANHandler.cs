using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.MAXBOTIX)]
    public class MaxbotixLoRaWANHandler : Handler, IHandler
    {
        public static byte[] HexToBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var version = request.Data.Substring(0, 2); //// First two characters are for version
                request.Data = request.Data.Remove(0, 2);

                var hexBytes = request.Data.SplitInParts(4).ToList();

                var deviceID = hexBytes[0].FromHexToDecimal();
                var flags = hexBytes[1].FromHexToDecimal();

                if (flags == 3 || flags == 1) //// Flag tell if distance is measured
                {
                    var distance = hexBytes[2].FromHexToDecimal() / 10;
                    var trials = hexBytes[3].FromHexToDecimal();
                    response.Measurements.Add(MeasumrentType.distance_cm.ToString(), distance);

                    var batteryPercent = request.Battery > 0 ? (int)((100d / 255d) * request.Battery) : 0;
                    response.Measurements.Add(MeasumrentType.battery_pct.ToString(), batteryPercent);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MaxbotixLoRaWAN Measurements failed", ex);
            }

            return response;
        }
    }
}
