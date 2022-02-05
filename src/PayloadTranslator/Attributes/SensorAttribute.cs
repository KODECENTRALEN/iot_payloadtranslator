using System;
using PayloadTranslator.Enums;

namespace PayloadTranslator.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SensorAttribute : Attribute
{
    public SensorAttribute()
    {
    }

    public SensorAttribute(DeviceTypes deviceType)
    {
        ModelId = "dtmi:generic:generic;1";
        DeviceType = deviceType;
    }

    public SensorAttribute(DeviceTypes deviceType, string possibleNames)
    {
        ModelId = "dtmi:generic:generic;1";
        DeviceType = deviceType;
        PossibleNames = possibleNames;
    }

    public SensorAttribute(DeviceTypes deviceType, string modelId, string possibleNames)
    {
        ModelId = modelId;
        DeviceType = deviceType;
        PossibleNames = possibleNames;
    }

    public string ModelId { get; set; }

    public string PossibleNames { get; set; }

    public DeviceTypes DeviceType { get; set; }
}

