using Enums;
using Entities;

namespace Handlers
{
    public interface IHandler
    {
        PayloadResponse HandlePayload(PayloadRequest request);
    }
}
