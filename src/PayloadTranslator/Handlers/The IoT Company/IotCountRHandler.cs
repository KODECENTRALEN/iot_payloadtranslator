using System;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using Attributes;
using Enums;
using Entities;
using Enums;

namespace Handlers
{
    [Sensor(DeviceTypes.CONNECTEDDETECTIFY)]
    public class IotCountRHandler : Handler, IHandler
    {
        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            var hexBytes = request.Data.SplitInParts(2).ToList();
            var binaryString = string.Join(string.Empty, request.Data.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            try
            {
                var counter = (hexBytes[0] + hexBytes[1] + hexBytes[2] + hexBytes[3]).FromHexToDecimal();
                response.Measurements.Add(MeasumrentType.count.ToString(), counter);
            }
            catch (Exception ex)
            {
                throw new Exception("IotCountRHandler failed", ex);
            }

            return response;
        }
    }
}
