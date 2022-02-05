using System;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers.Elsys
{
    [Sensor(DeviceTypes.ELT_HP_HP, "dtmi:iotplatform:elsysElt2Hp6rt;1", "ELT-HP-HP")]
    public class ElsysElt2HpHandler : Handler, IHandler
    {
        public static readonly ElsysPayloadDecoder Decoder = new ElsysPayloadDecoder();

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var result = Decoder.DecodePayload(request.Data);

                var temperature = result.Temperature.Value;
                var temperatureRoad = result.ExternalTemperature1.Value;
                var temperatureGround = result.ExternalTemperature2.Value;

                response.Measurements.Add(MeasumrentType.temperature_road_c.ToString(), temperatureRoad);
                response.Measurements.Add(MeasumrentType.temperature_ground_c.ToString(), temperatureGround);
                response.Measurements.Add(MeasumrentType.temperature_c.ToString(), temperature);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(ElsysElt2Handler)} failed", ex);
            }

            return response;
        }
    }
}
