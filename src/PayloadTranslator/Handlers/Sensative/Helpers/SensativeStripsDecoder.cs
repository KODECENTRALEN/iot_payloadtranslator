using System;

namespace Handlers.Sensative.Helpers
{
    public class SensativeStripsDecoder
    {
        //// Migrated from the Javascript Decoder provided by Sensative found on https://sensative.com/sensors/strips-lora-sensors/sensative-strips-lora-resource-center/
        public static SensativeValues Decoder(byte[] bytes, int port)
        {
            //// Decode an uplink message from a buffer
            //// (array) of bytes to an object of fields.
            var pos = 0;
            dynamic type;
            SensativeValues decoded = new SensativeValues();

            switch (port)
            {
                case 1:
                    if (bytes.Length < 2)
                    {
                        break;
                    }

                    decoded.HistSeqNr = (bytes[pos++] << 8) | bytes[pos++];
                    decoded.PrevHistSeqNr = decoded.HistSeqNr;
                    while (pos < bytes.Length - 1)
                    {
                        type = bytes[pos++];
                        var test = type & 0x80;
                        var typeBool = Convert.ToBoolean(type & 0x80);

                        if (typeBool)
                        {
                            decoded.PrevHistSeqNr--;
                        }

                        DecodeFrame(bytes, pos, type, ref decoded);
                    }

                    break;
            }

            return decoded;
        }

        private static void DecodeFrame(byte[] bytes, int pos, dynamic type, ref SensativeValues target)
        {
            switch (type & 0x7f)
            {
                case 0:
                    break;
                case 1: //// Battery 1byte 0-100%				
                    target.Battery = bytes[pos++];
                    break;
                case 2: //// TempReport 2bytes 0.1degree C	
                    var x = bytes[pos] & 0x80;
                    var xbool = Convert.ToBoolean(x);
                    target.Temperature = ((xbool ? 0xFFFF << 16 : 0) | (bytes[pos++] << 8) | bytes[pos++]) / 10d;
                    break;
                case 3:
                    //// Temp alarm
                    var highAlarmBool = Convert.ToBoolean(bytes[pos] & 0x01);
                    target.HighAlarm = !!highAlarmBool;
                    var lowAlarmBool = Convert.ToBoolean(bytes[pos] & 0x02);
                    target.LowAlarm = !!lowAlarmBool;
                    pos++;
                    break;
                case 4: //// AvgTempReport 2bytes 0.1degree C
                    var avgTemperatureBool = Convert.ToBoolean(bytes[pos] & 0x80);
                    target.AvgTemperature = ((avgTemperatureBool ? 0xFFFF << 16 : 0) | (bytes[pos++] << 8) | bytes[pos++]) / 10;
                    break;
                case 5:
                    //// AvgTemp alarm
                    var avgTempAlarmHighBool = Convert.ToBoolean(bytes[pos] & 0x01);
                    target.AvgTempAlarmHigh = !!avgTempAlarmHighBool;
                    var avgTempAlarmLowBool = Convert.ToBoolean(bytes[pos] & 0x02);
                    target.AvgTempAlarmLow = !!avgTempAlarmLowBool;
                    pos++;
                    break;
                case 6: //// Humidity 1byte 0-100% in 0.5%
                    target.Humidity = bytes[pos++] / 2d;
                    break;
                case 7: //// Lux 2bytes 0-65535lux
                    target.Lux = (bytes[pos++] << 8) | bytes[pos++];
                    break;
                case 8: //// Lux 2bytes 0-65535lux
                    target.Lux2 = (bytes[pos++] << 8) | bytes[pos++];
                    break;
                case 9: //// DoorSwitch 1bytes binary
                    var doorBool = Convert.ToBoolean(bytes[pos++]);
                    target.Door = !!doorBool;
                    break;
                case 10: //// DoorAlarm 1bytes binary
                    var doorAlarmBool = Convert.ToBoolean(bytes[pos++]);
                    target.DoorAlarm = !!doorAlarmBool;
                    break;
                case 11: //// TamperSwitch 1bytes binary
                    var tamperSwitchBool = Convert.ToBoolean(bytes[pos++]);
                    target.TamperSwitch = !!tamperSwitchBool;
                    break;
                case 12: //// TamperAlarm 1bytes binary
                    var tamperAlarmhBool = Convert.ToBoolean(bytes[pos++]);
                    target.TamperAlarm = !!tamperAlarmhBool;
                    break;
                case 13: //// Flood 1byte 0-100%
                    target.Flood = bytes[pos++];
                    break;
                case 14: //// FloodAlarm 1bytes binary
                    var floodAlarmBool = Convert.ToBoolean(bytes[pos++]);
                    target.FloodAlarm = !!floodAlarmBool;
                    break;
                case 15: //// FoilAlarm 1bytes binary
                    var foilAlarmBool = Convert.ToBoolean(bytes[pos++]);
                    target.FoilAlarm = !!foilAlarmBool;
                    break;
                case 16: //// UserSwitchAlarm
                    var userSwitchBool = Convert.ToBoolean(bytes[pos++]);
                    target.UserSwitch = !!userSwitchBool;
                    break;
                case 17: //// Door count
                    target.DoorCount = (bytes[pos++] << 8) | bytes[pos++];
                    break;
                case 80:
                    target.CombinedHumidity = bytes[pos++] / 2;
                    var combinedTemperatureBool = Convert.ToBoolean(bytes[pos] & 0x80);
                    target.CombinedTemperature = ((combinedTemperatureBool ? 0xFFFF << 16 : 0) | (bytes[pos++] << 8) | bytes[pos++]) / 10;
                    break;
                case 81:
                    target.CombinedHumidity = bytes[pos++] / 2;
                    var combinedAvgTemperature = Convert.ToBoolean(bytes[pos] & 0x80);
                    target.CombinedAvgTemperature = ((combinedAvgTemperature ? 0xFFFF << 16 : 0) | (bytes[pos++] << 8) | bytes[pos++]) / 10;
                    break;
                case 82:
                    target.CombinedDoor = !!Convert.ToBoolean(bytes[pos++]);
                    var combinedTempBool = Convert.ToBoolean(bytes[pos] & 0x80);
                    target.CombinedTemperature = ((combinedTempBool ? 0xFFFF << 16 : 0) | (bytes[pos++] << 8) | bytes[pos++]) / 10;
                    break;
            }
        }
    }
}
