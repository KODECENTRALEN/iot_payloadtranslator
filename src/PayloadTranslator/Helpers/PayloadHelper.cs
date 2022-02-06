using Bluefragments.Utilities.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayloadTranslator.Entities;
using Utilities;

namespace PayloadTranslator.Helpers;

public static class PayloadHelper
{
    private enum Properties
    {
        Device = 0,
        Data,
        Time,
        Battery,
        Parsed,
        Content,
    }

    public static PayloadRequest GeneratePayloadRequest(dynamic payload)
    {
        string deviceId;
        long time;

        deviceId = GetValueFromPayload<string>(payload, Properties.Device);
        if (deviceId == null)
        {
            return null;
        }

        var innerPayload = DynamicInspector.GetDynamicValue<dynamic>(payload, PropertyNames.PayloadProperties);
        if (innerPayload != null)
        {
            payload = innerPayload;
        }

        var data = (string)DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DataProperties);

        var timeString = (string)GetValueFromPayload<string>(payload, Properties.Time);
        var dateTime = DateTimeHelper.ConvertToDateTime(timeString);
        time = dateTime.ToEpochTimeSeconds();

        return new PayloadRequest()
        {
            Data = data,
            Time = time,
            Payload = payload,
            DeviceId = deviceId
        };
    }

    private static T GetValueFromPayload<T>(dynamic payload, Properties property)
    {
        switch (property)
        {
            case Properties.Data:
                return (T)DynamicInspector.GetDynamicValue<T>(payload, PropertyNames.DataProperties);
            case Properties.Device:
                return (T)DynamicInspector.GetDynamicValue<T>(payload, PropertyNames.DeviceIdProperties);
            case Properties.Time:
                return (T)DynamicInspector.GetDynamicValue<T>(payload, PropertyNames.TimeProperties);
            case Properties.Battery:
                return (T)DynamicInspector.GetDynamicValue<T>(payload, PropertyNames.BatteryProperties);
            case Properties.Content:
                return (T)DynamicInspector.GetDynamicValue<T>(payload, PropertyNames.ContentProperties);
            default:
                break;
        }

        return default;
    }

    private static long GetDynamicTimestampFromChild(dynamic data, List<string> possibleDataPropertyNames, List<string> possibleTimePropertyNames)
    {
        if (data == null)
        {
            return default;
        }

        foreach (var possibleDataPropertyName in possibleDataPropertyNames)
        {
            try
            {
                var value = data[possibleDataPropertyName];
                if (value == null)
                {
                    continue;
                }

                dynamic dataItem = null;

                if (value is JArray)
                {
                    var dataArray = value.ToObject<List<dynamic>>();
                    if (dataArray == null)
                    {
                        continue;
                    }

                    dataItem = ((List<dynamic>)dataArray).FirstOrDefault();
                }
                else if (value is JObject)
                {
                    dataItem = value.ToObject<dynamic>();
                    if (dataItem == null)
                    {
                        continue;
                    }
                }

                string timeString = DynamicInspector.GetDynamicValue<string>(dataItem, possibleTimePropertyNames);
                if (timeString == null)
                {
                    continue;
                }
                else if (timeString.IndexOf("-") > 0)
                {
                    if (DateTime.TryParse(timeString, out var time))
                    {
                        return time.ToEpochTimeSeconds();
                    }

                    continue;
                }
                else
                {
                    long.TryParse(timeString, out var time);
                    if (time == 0)
                    {
                        continue;
                    }

                    var longDate = time.FromNumericDateTimeValue();
                    return longDate.ToEpochTimeSeconds();
                }
            }
            catch
            {
                continue;
            }
        }

        return default;
    }
}
