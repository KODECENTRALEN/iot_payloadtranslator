using System;
using System.Collections.Generic;
using System.Linq;
using Bluefragments.Utilities.Extensions;
using Newtonsoft.Json.Linq;
using PayloadTranslator.Entities;
using Utilities;

namespace PayloadTranslator.Handlers;

public abstract class Handler
{
    public virtual PayloadResponse HandlePayload(PayloadRequest request)
    {
        return default;
    }
}
