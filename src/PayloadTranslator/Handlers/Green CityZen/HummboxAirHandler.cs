using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using Helpers;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.HUMMBOXAIR)]
    public class HummboxAirHandler : Handler, IHandler
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
                    //// doc says do little Endian. aka reverse order on all
                    var temp = (hexBytes[2] + hexBytes[1]).FromHexToDouble();
                    var temperature = temp / 10;
                    var humidity = (hexBytes[4] + hexBytes[3]).FromHexToDouble();
                    var lux = (hexBytes[6] + hexBytes[5]).FromHexToDouble();
                    var pir = hexBytes[7].FromHexToDouble();
                    var co2 = (hexBytes[9] + hexBytes[8]).FromHexToDouble();
                    //// according to email not official docs
                    var co2Level = co2 * 10;
                    var battery = hexBytes[10].FromHexToDouble();
                    var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);

                    response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                    response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                    response.Measurements.Add(MeasumrentType.lux_lumen.ToString(), lux);
                    response.Measurements.Add(MeasumrentType.movement.ToString(), pir);
                    response.Measurements.Add(MeasumrentType.co2_ppm.ToString(), co2Level);
                    response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                    response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HummBoxAirMeasurements failed", ex);
            }

            return response;
        }
    }
}
