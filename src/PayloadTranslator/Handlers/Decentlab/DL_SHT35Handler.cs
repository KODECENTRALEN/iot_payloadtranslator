using System;
using Helpers;
using Attributes;
using Enums;
using Entities;
using Handlers.Decentlab.Helpers;
using Enums;

namespace Handlers.Decentlab
{
    [Sensor(DeviceTypes.DL_SHT35)]
    public class DL_SHT35Handler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var decoded = DL_SHT35_DECODER.Decode(request.Data);
                var temperature = 0d;
                var humidity = 0d;
                foreach (var rec in decoded)
                {
                    switch (rec.Key)
                    {
                        case "Air temperature":
                            response.Measurements.Add(MeasumrentType.temperature_c.ToString(), rec.Value.Item1);
                            break;
                        case "Air humidity":
                            response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), rec.Value.Item1);
                            break;
                        default:
                            break;
                    }
                }

                var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);
                response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);

                var batteryPercent = request.Battery > 0 ? (int)((100d / 255d) * request.Battery) : 0;
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), batteryPercent);
            }
            catch (Exception ex)
            {
                throw new Exception("DL_SHT35 Measurements failed", ex);
            }

            return response;
        }
    }
}
