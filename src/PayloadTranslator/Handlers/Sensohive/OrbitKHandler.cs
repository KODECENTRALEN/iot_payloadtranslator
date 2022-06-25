using System;
using System.Globalization;
using Attributes;
using Enums;
using Entities;
using Enums;

namespace Handlers
{
    [Sensor(DeviceTypes.ORBITK)]
    public class OrbitKHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var culture = new CultureInfo("en-US");
                var values = request.Data.Split(',');
                var temperatureAir = double.Parse(values[1], culture);
                var temperatureRoad = double.Parse(values[3], culture);
                response.Measurements.Add(MeasumrentType.temperature_road_c.ToString(), temperatureRoad);
            }
            catch (Exception ex)
            {
                throw new Exception("OrbitKMeasurements failed", ex);
            }

            return response;
        }
    }
}
