using System;

namespace Handlers.Sensative.Helpers
{
    public class SensativeValues
    {
        public DateTime HistoryNow { get; set; }

        public int HistSeqNr { get; set; }

        public int PrevHistSeqNr { get; set; }

        public int Battery { get; set; }

        public double Temperature { get; set; }

        public bool HighAlarm { get; set; }

        public bool LowAlarm { get; set; }

        public double AvgTemperature { get; set; }

        public bool AvgTempAlarmHigh { get; set; }

        public bool AvgTempAlarmLow { get; set; }

        public double Humidity { get; set; }

        public int Lux { get; set; }

        public int Lux2 { get; set; }

        public bool Door { get; set; }

        public bool DoorAlarm { get; set; }

        public bool TamperSwitch { get; set; }

        public bool TamperAlarm { get; set; }

        public int Flood { get; set; }

        public bool FloodAlarm { get; set; }

        public bool FoilAlarm { get; set; }

        public bool UserSwitch { get; set; }

        public int DoorCount { get; set; }

        public double CombinedHumidity { get; set; }

        public double CombinedTemperature { get; set; }

        public double CombinedAvgTemperature { get; set; }

        public bool CombinedDoor { get; set; }
    }
}
