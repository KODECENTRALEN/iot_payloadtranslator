using Data.Enums;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.ERSSOUND, "dtmi:generic:generic;1", "ERSSOUND")]
    public class ElsysSoundHandler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.ERSSOUND;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                var soundAverage = result.Sound.SoundAverage;
                var soundPeak = result.Sound.SoundPeak;
                var temperature = result.Temperature.Value;
                var humidity = result.Humidity.Value;

                var bat = request.Battery * 100 / 254;
                var battery = Math.Round((double)bat, 2);

                response.Measurements.Add(MeasurementType.sound_avg_db.ToString(), (double)soundAverage);
                response.Measurements.Add(MeasurementType.sound_peak_db.ToString(), (double)soundPeak);
                response.Measurements.Add(MeasurementType.temperature_c.ToString(), temperature);
                response.Measurements.Add(MeasurementType.battery_pct.ToString(), battery);
                response.Measurements.Add(MeasurementType.humidity_pct.ToString(), humidity);
            }
            catch (Exception ex)
            {
                throw new Exception("ElsysSoundMeasurements failed", ex);
            }

            return response;
        }
    }
}
