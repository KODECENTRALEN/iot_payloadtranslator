using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PayloadTranslator.Handlers.Decentlab.Helpers
{
    //// Copied from https://github.com/decentlab/decentlab-decoders
    public class DL_WRM_Decoder
    {
        public const int PROTOCOLVERSION = 2;

        private static readonly List<Sensor> SENSORS = new List<Sensor>()
        {
            new Sensor(2, new List<SensorValue>()
            {
              new SensorValue("Air temperature", "°C", x => (175 * x[0] / 65535) - 45),
              new SensorValue("Air humidity", "%", x => 100 * x[1] / 65535),
            }),
            new Sensor(2, new List<SensorValue>()
            {
              new SensorValue("Surface temperature", "°C", x => (x[0] - 1000) / 10),
              new SensorValue("Head temperature", "°C", x => (x[1] - 1000) / 10),
            }),
            new Sensor(1, new List<SensorValue>()
            {
              new SensorValue("Battery voltage", "V", x => x[0] / 1000),
            }),
        };

        internal delegate double Conversion(double[] x);

        public static Dictionary<string, Tuple<double, string>> Decode(byte[] msg)
        {
            return Decode(new MemoryStream(msg));
        }

        public static Dictionary<string, Tuple<double, string>> Decode(string msg)
        {
            byte[] output = new byte[msg.Length / 2];
            for (int i = 0, j = 0; i < msg.Length; i += 2, j++)
            {
                output[j] = (byte)int.Parse(msg.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return Decode(output);
        }

        public static Dictionary<string, Tuple<double, string>> Decode(Stream msg)
        {
            var version = msg.ReadByte();
            if (version != PROTOCOLVERSION)
            {
                throw new InvalidDataException("protocol version " + version + " doesn't match v2");
            }

            var result = new Dictionary<string, Tuple<double, string>>();
            result["Protocol version"] = new Tuple<double, string>(version, null);

            var deviceId = ReadInt(msg);
            result["Device ID"] = new Tuple<double, string>(deviceId, null);

            var flags = ReadInt(msg);
            foreach (var sensor in SENSORS)
            {
                if ((flags & 1) == 1)
                {
                    double[] x = new double[sensor.Length];
                    for (int i = 0; i < sensor.Length; i++)
                    {
                        x[i] = ReadInt(msg);
                    }

                    foreach (var val in sensor.Values)
                    {
                        if (val.Convert == null)
                        {
                            continue;
                        }

                        result[val.Name] = new Tuple<double, string>(val.Convert(x), val.Unit);
                    }
                }

                flags >>= 1;
            }

            return result;
        }

        private static int ReadInt(Stream stream)
        {
            return (stream.ReadByte() << 8) + stream.ReadByte();
        }

        internal class SensorValue
        {
#pragma warning disable SA1401 // Fields should be private
            internal readonly Conversion Convert;
#pragma warning restore SA1401 // Fields should be private

            internal SensorValue(string name, string unit, Conversion convert)
            {
                Name = name;
                Unit = unit;
                Convert = convert;
            }

            internal string Name { get; set; }

            internal string Unit { get; set; }
        }

        private class Sensor
        {
            internal Sensor(int length, List<SensorValue> values)
            {
                Length = length;
                Values = values;
            }

            internal int Length { get; set; }

            internal List<SensorValue> Values { get; set; }
        }
    }
}
