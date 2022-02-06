using System.Collections.Generic;

namespace PayloadTranslator.Entities;

public static class PropertyNames
{
    public static List<string> DeviceIdProperties { get; } = new List<string>() { "dev_eui", "connectionDeviceId", "device", "Device", "EUI", "hardware_serial", "stationId", "rsuid", "id", "deviceId", "DeviceId" };

    public static List<string> DeviceTypeProperties { get; } = new List<string>() { "deviceType", "devicetype" };

    public static List<string> PayloadProperties { get; } = new List<string>() { "Payload" };

    public static List<string> ContentProperties { get; } = new List<string>() { "Body", "body" };

    public static List<string> DataProperties { get; } = new List<string>() { "data", "Data", "rawdata", "Rawdata", "payload_raw", "metadata", "measurements" };

    public static List<string> TimeProperties { get; } = new List<string>() { "timestamp", "ts", "time", "Time", "timeObserved", "tstamp" };

    public static List<string> BatteryProperties { get; } = new List<string>() { "bat", "battery", "Battery" };
}
