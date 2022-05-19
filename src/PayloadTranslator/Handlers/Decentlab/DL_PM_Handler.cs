using System;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers.Decentlab.Helpers;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.DL_PM)]
    public class DL_PM_Handler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var decoded = DL_PM_Decoder.Decode(request.Data);
                foreach (var rec in decoded)
                {
                    switch (rec.Key)
                    {
                        case "PM1.0 mass concentration":
                            response.Measurements.Add(MeasurementType.mass_microgram_pm1.ToString(), rec.Value.Item1);
                            break;
                        case "PM2.5 mass concentration":
                            response.Measurements.Add(MeasurementType.mass_microgram_pm2_5.ToString(), rec.Value.Item1);
                            break;
                        case "PM4 mass concentration":
                            response.Measurements.Add(MeasurementType.mass_microgram_pm4.ToString(), rec.Value.Item1);
                            break;
                        case "PM10 mass concentration":
                            response.Measurements.Add(MeasurementType.mass_microgram_pm10.ToString(), rec.Value.Item1);
                            break;
                        case "Typical particle size":
                            response.Measurements.Add(MeasurementType.particlesize_nm.ToString(), rec.Value.Item1);
                            break;
                        case "PM0.5 number concentration":
                            response.Measurements.Add(MeasurementType.count_pm0_5.ToString(), rec.Value.Item1);
                            break;
                        case "PM1.0 number concentration":
                            response.Measurements.Add(MeasurementType.count_pm1.ToString(), rec.Value.Item1);
                            break;
                        case "PM2.5 number concentration":
                            response.Measurements.Add(MeasurementType.count_pm2_5.ToString(), rec.Value.Item1);
                            break;
                        case "PM4 number concentration":
                            response.Measurements.Add(MeasurementType.count_pm4.ToString(), rec.Value.Item1);
                            break;
                        case "PM10 number concentration":
                            response.Measurements.Add(MeasurementType.count_pm10.ToString(), rec.Value.Item1);
                            break;
                        case "Air temperature":
                            response.Measurements.Add(MeasurementType.temperature_c.ToString(), rec.Value.Item1);
                            break;
                        case "Air humidity":
                            response.Measurements.Add(MeasurementType.humidity_pct.ToString(), rec.Value.Item1);
                            break;
                        case "Barometric pressure":
                            var pressure = Math.Round((double)(rec.Value.Item1 / 100), 1);
                            response.Measurements.Add(MeasurementType.pressure_hpa.ToString(), pressure);
                            break;
                        default:
                            break;
                    }
                }

                var batteryPercent = request.Battery > 0 ? (int)((100d / 255d) * request.Battery) : 0;
                response.Measurements.Add(MeasurementType.battery_pct.ToString(), batteryPercent);
            }
            catch (Exception ex)
            {
                throw new Exception("DL_PM Measurements failed", ex);
            }

            return response;
        }
    }
}
