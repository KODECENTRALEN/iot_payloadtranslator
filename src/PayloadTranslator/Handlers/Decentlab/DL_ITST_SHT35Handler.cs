﻿using System;
using Attributes;
using Enums;
using Entities;
using Handlers.Decentlab.Helpers;
using Enums;

namespace Handlers
{
    [Sensor(DeviceTypes.DL_ITST_SHT35)]
    public class DL_ITST_SHT35Handler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var decoded = DL_WRM_Decoder.Decode(request.Data);
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
                        case "Surface temperature":
                            response.Measurements.Add(MeasumrentType.temperature_road_c.ToString(), rec.Value.Item1);
                            break;
                        case "Head temperature":
                            response.Measurements.Add(MeasumrentType.temperature_head_c.ToString(), rec.Value.Item1);
                            break;
                        default:
                            break;
                    }
                }

                var batteryPercent = request.Battery > 0 ? (int)((100d / 255d) * request.Battery) : 0;
                response.Measurements.Add(MeasumrentType.battery_pct.ToString(), batteryPercent);
            }
            catch (Exception ex)
            {
                throw new Exception("DL_ITST_SHT35 Measurements failed", ex);
            }

            return response;
        }
    }
}
