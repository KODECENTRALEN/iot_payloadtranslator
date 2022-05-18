using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using Data.Enums;

namespace PayloadTranslator.Handlers
{
    [Sensor(DeviceTypes.COUNTR1)]
    public class CountR1Handler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                //// documentation states do little Endian. aka reverse order of second and third bytes
                var counter = (hexBytes[2] + hexBytes[1]).FromHexToDouble();
                response.Measurements.Add(MeasurementType.count.ToString(), counter);
            }
            catch (Exception ex)
            {
                throw new Exception("CountR1Measurements failed", ex);
            }

            return response;
        }
    }
}
