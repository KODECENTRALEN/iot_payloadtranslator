using System;
using System.Globalization;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.sensit, "dtmi:iotplatform:sigfoxSensit211r7;1", "sensit")]
    public class SensitHandler : Handler, IHandler
    {
        /// <summary>
        /// Documentation can be found at https://storage.googleapis.com/public-assets-xd-sigfox-production-338901379285/build/4059ab1jy7g2v9l/sensit%20v2%20frames%20uplink.pdf
        /// </summary>
        /// <param name="request"></param>
        /// <returns>PayloadResponse.</returns>
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var culture = new CultureInfo("en-US");

                var hexBytes = request.Data.SplitInParts(2).ToList();
                var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                var bytes = binaryString.SplitInParts(8).ToList();

                var batteryPayload = string.Concat(bytes[0].Substring(0, 1), bytes[1].Substring(0, 4)).FromBinaryToDecimal();
                var batteryVolts = (batteryPayload * 0.05d) + 2.7;
                var batteryPct = Math.Round(100 / 4.15 * batteryVolts, 0);

                response.Measurements.Add(MeasumrentType.battery_level.ToString(), batteryVolts);
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), batteryPct);

                var mode = bytes[0].Substring(5, 3).FromBinaryToDecimal();

                switch (mode)
                {
                    case 0:
                    case 1:
                        var temperatureDecimal = string.Concat(bytes[1].Substring(0, 4), bytes[2].Substring(2, 6)).FromBinaryToDecimal();
                        var temperatureC = Math.Round((temperatureDecimal - 200) / 8);
                        response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperatureC);
                        var humidityPayload = bytes[3].Substring(0, 8).FromBinaryToDecimal();
                        var humidityPct = Math.Round(humidityPayload / 2, 0);
                        response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidityPct);
                        break;
                    case 2:
                        var brightnessPayload = binaryString.Substring(16, 16).FromBinaryToDecimal();
                        var brightnessLux = Math.Round(brightnessPayload / 96, 2);
                        response.Measurements.Add(MeasumrentType.lux_lumen.ToString(), brightnessLux);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Sensit measurement failed", ex);
            }

            return response;
        }
    }
}
