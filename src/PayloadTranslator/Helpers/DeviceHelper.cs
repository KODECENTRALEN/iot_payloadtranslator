using System.Reflection;
using PayloadTranslator.Attributes;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers;
using Utilities;

namespace PayloadTranslator.Helpers;

public static class DeviceHelper
{
    public static IHandler? FindHandlerForDeviceType(string deviceType, IEnumerable<DeviceAttributeContainer> attributes)
    {
        if (deviceType is not null)
        {
            foreach (var possibleHandlerType in attributes)
            {
                Type type = possibleHandlerType.Type;
                SensorAttribute? SensorType = possibleHandlerType.Attributes?.FirstOrDefault();

                if (SensorType is not null)
                {
                    string[]? possibleNames = SensorType.PossibleNames?.Split(';');
                    if (possibleNames == null || possibleNames.Any() == false)
                    {
                        possibleNames = new string[] { SensorType.DeviceType.ToString() };
                    }

                    if (possibleNames is not null && possibleNames.Length > 0)
                    {
                        foreach (string possibleName in possibleNames)
                        {
                            if (possibleName.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (typeof(IHandler).IsAssignableFrom(type))
                                {
                                    var NewObject = Activator.CreateInstance(type);
                                    if (NewObject is not null)
                                    {
                                        IHandler? handlerInstance = (IHandler)NewObject;
                                        return handlerInstance;
                                    }
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
        var content = DynamicInspector.GetDynamicValue<dynamic>(payload, PropertyNames.ContentProperties);
        if (content != null) payload = content;

        string deviceId = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DeviceIdProperties);
        string deviceType = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DeviceTypeProperties);
        string data = DynamicInspector.GetDynamicValue<string>(payload, PropertyNames.DataProperties);
        long time = DynamicInspector.GetDynamicValue<long>(payload, PropertyNames.TimeProperties);
        int battery = DynamicInspector.GetDynamicValue<int>(payload, PropertyNames.BatteryProperties);

        return new PayloadRequest
        {
            DeviceType = deviceType,
            DeviceId = deviceId,
            Data = data,
            Battery = battery,
            Time = time,
        };
    }

    public static IEnumerable<DeviceAttributeContainer> GetTypesWithDeviceAttributes()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var types = currentAssembly.GetTypes();

        var typesWithMyAttribute =
            from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let attributes = t.GetCustomAttributes(typeof(SensorAttribute), true)
            where attributes != null && attributes.Length > 0
            select new DeviceAttributeContainer()
            {
                Type = t,
                Attributes = attributes.Cast<SensorAttribute>()
            };

        return typesWithMyAttribute;
    }
}

