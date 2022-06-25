using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Attributes;
using Enums;
using Entities;
using Handlers.Ranch_Systems.Entities;
using Enums;

namespace Handlers.Ranch_Systems
{
    [Sensor(DeviceTypes.RS130)]
    public class RS130Handler : Handler, IHandler
    {
        private const int WATERLEVELINMILIMIETERTYPE = 281;
        private const int TEMPERATUREFAHRENHEITTYPE = 21;

        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.RS130;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = JsonConvert.DeserializeObject<List<RSMeasurement>>(request.Data);

                var rawWaterLevelInMm = result.FirstOrDefault(x => x.Type == WATERLEVELINMILIMIETERTYPE)?.Value;

                if (rawWaterLevelInMm != null)
                {
                    double waterLevel;
                    if (rawWaterLevelInMm > 0)
                    {
                        var waterLevelInCm = rawWaterLevelInMm / 10D;
                        waterLevel = Math.Round((double)waterLevelInCm, 2);
                    }
                    else
                    {
                        waterLevel = 0;
                    }

                    response.Measurements.Add(MeasumrentType.distance_cm.ToString(), waterLevel);
                }

                var temperatureFahrenheit = result.FirstOrDefault(x => x.Type == TEMPERATUREFAHRENHEITTYPE)?.Value;
                if (temperatureFahrenheit != null)
                {
                    var temperatureCelcius = (temperatureFahrenheit - 32) * 5 / 9;
                    var temperature = Math.Round((double)temperatureCelcius, 2);
                    response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ranch System RS130 Measurements failed", ex);
            }

            return response;
        }
    }
}
