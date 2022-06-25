using Entities;

namespace Handlers;

public abstract class Handler
{
    public virtual PayloadResponse HandlePayload(PayloadRequest request)
    {
        return default;
    }
}
