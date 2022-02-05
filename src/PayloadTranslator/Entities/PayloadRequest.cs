namespace PayloadTranslator.Entities;

public class PayloadRequest
{
    public string DeviceId { get; set; }

    public string ModelId { get; set; }

    public string Data { get; set; }

    public long Time { get; set; }

    public int Battery { get; set; }

    public string DeviceType { get; set; }

    public dynamic Payload { get; set; }
}
