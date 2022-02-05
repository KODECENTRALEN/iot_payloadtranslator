using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PayloadTranslator.Attributes;
using PayloadTranslator.Handlers;
using Utilities;

namespace PayloadTranslator.Entities;

public static class DeviceHelper
{
    public static IHandler FindHandlerForDeviceType(string deviceType, IEnumerable<DeviceAttributeContainer> attributes)
    {
        if (deviceType is not null)
        {
            foreach (var possibleHandlerType in attributes)
            {
                var type = possibleHandlerType.Type;
                var attribute = possibleHandlerType.Attributes?.FirstOrDefault();

                if (attribute is not null)
                {
                    var possibleNames = attribute.PossibleNames.Split(';');
                    if (possibleNames is not null && possibleNames.Length > 0)
                    {
                        foreach (var possibleName in possibleNames)
                        {
                            if (possibleName.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (typeof(IHandler).IsAssignableFrom(type))
                                {
                                    var handlerInstance = (IHandler)Activator.CreateInstance(type);
                                    return handlerInstance;
                                }
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public static PayloadRequest DecodePayloadMessage(dynamic payload)
    {
        string deviceId = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DeviceIdProperties);
        string deviceType = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DeviceTypeProperties);
        string data = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DataProperties);
        long time = DynamicInspector.GetDynamicValue<long>(payload, PropertyNames.TimeProperties);
        int battery = DynamicInspector.GetDynamicValue<int>(payload, PropertyNames.BatteryProperties);

        var rawPayload = payload.payload != null ? JsonConvert.DeserializeObject<dynamic>(payload.payload?.ToString()) : null;

        //Enum.TryParse(deviceType, out DeviceTypes convertedDeviceType);

        var response = new PayloadRequest
        {
            DeviceType = deviceType,
            DeviceId = deviceId,
            Data = data,
            Battery = battery,
            Time = time,
            Payload = rawPayload,
        };

        return response;
    }

    public static string GetDeviceId(dynamic payload)
    {
        return DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DeviceIdProperties);
    }

    private static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(typeof(SensorAttribute), true).Length > 0)
            {
                yield return type;
            }
        }
    }
}

