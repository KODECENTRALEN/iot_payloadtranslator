using Bluefragments.Utilities.Extensions;
using Enums;
using Helpers;
using Attributes;
using Entities;
using Enums;
using Handlers.Robeau.Helpers;

namespace Handlers.Robeau
{
    [Sensor(DeviceTypes.ROBEAU)]
    public class RobeauVolumeterHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var headerValue = request.Data.Substring(0, 2);
                var hexString = request.Data.Substring(2, request.Data.Count() - 2);

                // Header values
                var binaryString = ExtractHeaderBits(headerValue);
                var batteryLevel = RobeauValueConverter.ConvertBatteryPercentage(binaryString.Substring(6, 2));

                // Sensors values
                var sensorValues = ExtractSensorValues(hexString);

                response.Measurements.Add(MeasumrentType.battery_level.ToString(), batteryLevel);
                response.Measurements.Add(MeasumrentType.count_dm1_tick.ToString(), sensorValues.Where(x => x.Key == 1).First().Value);
                response.Measurements.Add(MeasumrentType.count_dm2_tick.ToString(), sensorValues.Where(x => x.Key == 2).First().Value);
                response.Measurements.Add(MeasumrentType.count_dm3_tick.ToString(), sensorValues.Where(x => x.Key == 3).First().Value);
                response.Measurements.Add(MeasumrentType.count_dm4_tick.ToString(), sensorValues.Where(x => x.Key == 4).First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(RobeauVolumeterHandler)} failed", ex);
            }

            return response;
        }

        private string ExtractHeaderBits(string headerValue)
        {
            headerValue.ThrowIfParameterIsNullOrWhiteSpace(nameof(headerValue));

            var bytes = HexHelper.HexToBytes(headerValue);

            // go through bytes and convert to bit
            string binaryString = string.Empty;
            foreach (byte singleByte in bytes)
            {
                binaryString += Convert.ToString(singleByte, 2);
            }

            // we expect 8 bits, add 0's if needed
            while (binaryString.Length < 8)
            {
                binaryString = "0" + binaryString;
            }

            return binaryString;
        }

        private Dictionary<int, long> ExtractSensorValues(string hexString)
        {
            hexString.ThrowIfParameterIsNullOrWhiteSpace(nameof(hexString));
            if (hexString.Length != 12 * 4)
            {
                throw new ArgumentException("Sensor data should contain 48 digits");
            }

            var hexes = hexString.SplitInParts(12);

            var sensorTicksDictionary = new Dictionary<int, long>();
            int counter = 1;
            foreach (var hex in hexes)
            {
                long ticks = 0;
                var passed = long.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out ticks);

                if (passed)
                {
                    sensorTicksDictionary.Add(counter, ticks);
                }

                counter++;
            }

            return sensorTicksDictionary;
        }
    }
}
