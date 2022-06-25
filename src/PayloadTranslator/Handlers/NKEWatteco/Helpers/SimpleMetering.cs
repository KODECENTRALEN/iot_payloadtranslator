using System;
using System.Collections.Generic;
using System.Text;

namespace Handlers.NKEWatteco.Helpers
{
    public class SimpleMetering
    {
        public int ActiveEnergy { get; set; }

        public int ReActiveEnergy { get; set; }

        public int NumberOfSamples { get; set; }

        public int ActivePower { get; set; }

        public int ReActivePower { get; set; }
    }
}
