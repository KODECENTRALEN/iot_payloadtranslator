using PayloadTranslator.Enums;
using PayloadTranslator.Entities;

namespace PayloadTranslator.Handlers
{
    public interface IHandler
    {
        PayloadResponse HandlePayload(dynamic payload, string deviceType, string modelId);
    }
}
