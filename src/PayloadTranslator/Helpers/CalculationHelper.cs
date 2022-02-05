using System;
using Bluefragments.Utilities.Extensions;

namespace Helpers
{
    public static class CalculationHelper
    {
        public static double CalculateDewPoint(double temperature, double humidity)
        {
            double a = 17.62;
            double b = 243.12;
            double x = Math.Log(humidity / 100) + (a * temperature / (b + temperature));
            double d = b * x / (a - x);
            return Math.Round(d, 2);
        }
    }
}
