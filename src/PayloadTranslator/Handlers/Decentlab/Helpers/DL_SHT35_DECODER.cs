using System;
using System.Collections.Generic;
using System.IO;

namespace PayloadTranslator.Handlers.Decentlab.Helpers
{
    public class DL_SHT35_DECODER
    {
        public const int PROTOCOLVERSION = 2;

        private static readonly List<Sensor> SENSORS = new List<Sensor>()
        {
            new Sensor(2, new List<SensorValue>()
            {
                new SensorValue("Air temperature", "°C", x => (175 * x[0] / 65535) - 45),
                new SensorValue("Air humidity", "%", x => 100 * x[1] / 65535),
            }),
            new Sensor(1, new List<SensorValue>()
            {
              new SensorValue("Battery voltage", "V", x => x[0] / 1000),
            }),
        };

        private delegate double Conversion(double[] x);

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
                        if (val.Convert != null)
                        {
                            result[val.Name] = new Tuple<double, string>(val.Convert(x), val.Unit);
                        }
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

        private class SensorValue
        {
            internal SensorValue(string name, string unit, Conversion convert)
            {
                Name = name;
                Unit = unit;
                Convert = convert;
            }

            internal Conversion Convert { get; set; }

            internal string Name { get; set; }

            internal string Unit { get; set; }
        }
    }
}
