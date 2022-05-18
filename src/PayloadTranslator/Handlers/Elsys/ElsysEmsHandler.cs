using Data.Enums;
using Helpers;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.EMS, "dtmi:generic:generic;1", "ems")]
    public class ElsysEmsHandler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                if (result.Temperature != null)
                {
                    var temperature = result.Temperature.Value;
                    response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
                }

                if (result.Humidity != null)
                {
                    var humidity = result.Humidity.Value;
                    response.Measurements.Add(MeasumrentType.humidity_pct.ToString(), humidity);
                }

                if (result.Temperature != null && result.Humidity != null)
                {
                    var temperature = result.Temperature.Value;
                    var humidity = result.Humidity.Value;
                    var dewpoint = CalculationHelper.CalculateDewPoint(temperature, humidity);
                    response.Measurements.Add(MeasumrentType.dewpoint_c.ToString(), dewpoint);
                }

                /* Number of times the reed switch has been triggered since last send.
                 * 
                 * In my one example data, this was undefined. I am guessing that this was
                 * because it was 0. If this is true, then the value should possibly be
                 * set to 0 unstead of undefined in the output.
                 * */
                if (result.PulseInput1 is not null)
                {
                    response.Measurements.Add(MeasumrentType.count.ToString(), result.PulseInput1);
                }
                //Number of times the reed switch has been triggered in total.
                if (result.PulseInput1Absolute is not null)
                {
                    response.Measurements.Add(MeasumrentType.count_acc.ToString(), result.PulseInput1Absolute);
                }


                if (request.Battery > 0)
                {
                    var bat = request.Battery * 100 / 254;
                    var battery = Math.Round((double)bat, 2);
                    response.Measurements.Add(MeasumrentType.battery_pct.ToString(), battery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(ElsysElt2Handler)} failed", ex);
            }

            return response;
        }
    }
}
