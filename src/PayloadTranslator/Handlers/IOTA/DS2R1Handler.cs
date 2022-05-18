using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.DS2R1, "dtmi:iotplatform:iotaDs2r126p;1", "DS2R1")]
    public class DS2R1Handler : Handler, IHandler
    {
        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.DS2R1;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var hexBytes = request.Data.SplitInParts(2).ToList();
                var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                var payloadString = request.Data.ToLower();

                if (hexBytes[0].StartsWith("c"))
                {
                    var batteryStatus = Convert.ToInt64(hexBytes[0], 16);
                    var batteryPct = (100 / 201d) * batteryStatus;
                    response.Measurements.Add(MeasurementType.battery_pct.ToString(), batteryPct);
                }

                if (hexBytes[0] == "aa")
                {
                    response.Measurements.Add(MeasurementType.door_open.ToString(), 1);
                }

                if (hexBytes[0] == "bb")
                {
                    response.Measurements.Add(MeasurementType.door_close.ToString(), 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DS2R1 measurements failed", ex);
            }

            return response;
        }
    }
}
