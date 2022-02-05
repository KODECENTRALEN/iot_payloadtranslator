using System;
using System.Collections.Generic;

namespace PayloadTranslator.Attributes;

public class DeviceAttributeContainer
{
    public Type Type { get; set; }

    public IEnumerable<SensorAttribute> Attributes { get; set; }
}
