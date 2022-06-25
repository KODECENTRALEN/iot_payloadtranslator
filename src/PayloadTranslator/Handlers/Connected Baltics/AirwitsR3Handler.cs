using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using Attributes;
using Enums;
using Helpers;
using Entities;
using Enums;

namespace Handlers
{
    [Sensor(DeviceTypes.AIRWITSR3)]
    public class AirwitsR3Handler : Handler, IHandler
    {
        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.AIRWITSR3;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                var temp = (hexBytes[0] + hexBytes[1]).FromHexToDouble();
                //// according to docs dividing by ten and substract forty to get temperature in Celsius
                var temperature = (temp / 10) - 40;
                var humidity = hexBytes[2].FromHexToDouble();
                var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);

                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);
            }
            catch (Exception ex)
            {
                throw new Exception("AirwitsCo2Measurements failed", ex);
            }

            return response;
        }
    }
}
