using System;
using Bluefragments.Utilities.Extensions;

namespace Handlers.Robeau.Helpers
{
    public static class RobeauValueConverter
    {
        public static int ConvertBatteryPercentage(string batteryLevel)
        {
            batteryLevel.ThrowIfParameterIsNullOrWhiteSpace(nameof(batteryLevel));

            int batteryPercentage;
            switch (batteryLevel.ToLower())
            {
                case "00":
                    batteryPercentage = 0;
                    break;
                case "01":
                    batteryPercentage = 25;
                    break;
                case "10":
                    batteryPercentage = 50;
                    break;
                case "11":
                    batteryPercentage = 100;
                    break;
                default:
                    throw new ArgumentException(nameof(batteryLevel));
            }

            return batteryPercentage;
        }
    }
}
