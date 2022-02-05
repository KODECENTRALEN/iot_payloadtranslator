using System.ComponentModel;

namespace PayloadTranslator.Enums;

public enum DeviceTypes
{
    [Description("unknown;unknown;unknown")]
    unknown,
    [Description("Connected Baltics;AIRWITSCO2;AIRWITSCO2")]
    AIRWITSCO2,
    [Description("Connected Baltics;AIRWITSR3;AIRWITSR3#airwitsr3temperature")]
    AIRWITSR3,
    [Description("Decentlab;ITST_SHT35;DL_ITST_SHT35")]
    DL_ITST_SHT35,
    [Description("Decentlab;DL_PM;DL_PM")]
    DL_PM,
    [Description("Decentlab;DL_SHT35;DL_SHT35")]
    DL_SHT35,
    [Description("Decentlab;MAXBOTIX;MAXBOTIX")]
    MAXBOTIX,
    [Description("Elsys;ELT2;ELT2")]
    ELT2,
    [Description("Elsys;ELT-HP-HP;ELT-HP-HP")]
    ELT_HP_HP,
    [Description("Elsys;ERSCO2;ERSCO2")]
    ERSCO2,
    [Description("Elsys;ERSSOUND;ERSSOUND")]
    ERSSOUND,
    [Description("Elsys;ULTRASONIC;ULTRASONIC")]
    ULTRASONIC,
    [Description("Green CityZen;HUMMBOXAIR;HUMMBOXAIR")]
    HUMMBOXAIR,
    [Description("Green CityZen;HUMMBOXWATER;HUMMBOXWATER")]
    HUMMBOXWATER,
    [Description("IoT Fabrikken;IOTFABRIKKEN;IOTFABRIKKEN")]
    IOTFABRIKKEN,
    [Description("IOTA;DS2R1;DS2R1")]
    DS2R1,
    [Description("IOTSU;S1M02;S1M02")]
    S1M02,
    [Description("NKEWatteco;SMARTPLUG;SMARTPLUG#INTENSO")]
    SMARTPLUG,
    [Description("Ranch Systems;RS130;RS130")]
    RS130,
    [Description("Robeau;ROBEAU;ROBEAU")]
    ROBEAU,
    [Description("Sensative;SENSATIVESTRIPS;SENSATIVESTRIPS")]
    SENSATIVESTRIPS,
    [Description("Sensohive;ORBITK;ORBITK")]
    ORBITK,
    [Description("Sigfox;sensit;sensit")]
    sensit,
    [Description("The IoT Company;COUNTR1;COUNTR1")]
    COUNTR1,
    [Description("The IoT Company;CONNECTEDDETECTIFY;CONNECTEDDETECTIFY")]
    CONNECTEDDETECTIFY,
    [Description("TST;TSBIN;TSBIN")]
    TSBIN,
    [Description("TST;TSWASTE;TSWASTE")]
    TSWASTE,
}

