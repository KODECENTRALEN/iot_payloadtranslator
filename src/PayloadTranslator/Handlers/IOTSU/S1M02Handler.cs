using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.S1M02, "dtmi:iotplatform:iotuS1m022f9;1", "S1M02")]
    public class S1M02Handler : Handler, IHandler
    {
        public bool CanHandle(DeviceTypes deviceType)
        {
            return deviceType == DeviceTypes.S1M02;
        }

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var hexBytes = request.Data.SplitInParts(2).ToList();
                var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                double voltage = ((double)hexBytes[0].FromHexToDecimal() * 25) / 1000;
                var batteryPct = (100 / 3.65d) * voltage;

                var activityCount1 = binaryString.Substring(16, 10).FromBinaryToDecimal();
                var activityCount2 = binaryString.Substring(26, 10).FromBinaryToDecimal();

                response.Measurements.Add(MeasurementType.battery_pct.ToString(), batteryPct);
                response.Measurements.Add(MeasurementType.count_dm1_activity_acc.ToString(), activityCount1);
                response.Measurements.Add(MeasurementType.count_dm2_activity_acc.ToString(), activityCount2);
            }
            catch (Exception ex)
            {
                throw new Exception("S1M02 measurements failed", ex);
            }

            return response;
        }
    }
}
