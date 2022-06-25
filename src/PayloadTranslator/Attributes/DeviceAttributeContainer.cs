using System;
using System.Collections.Generic;

namespace Attributes;

public class DeviceAttributeContainer
{
    public Type Type { get; set; }

    public IEnumerable<SensorAttribute> Attributes { get; set; }
}
