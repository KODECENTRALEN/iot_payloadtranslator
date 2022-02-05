using System;
using System.Collections.Generic;
using System.Text;

namespace PayloadTranslator.Handlers.NKEWatteco.Helpers
{
    public enum Mode
    {
        Unused = 0,
        Delta = 1,
        Threshold = 2,
        Reserved = 3,
    }
}
