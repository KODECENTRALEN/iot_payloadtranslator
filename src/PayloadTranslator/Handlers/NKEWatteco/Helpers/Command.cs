namespace PayloadTranslator.Handlers.NKEWatteco.Helpers
{
    public enum Command
    {
        ReadAttribute = 0x00,
        ReadAttributeResponse = 0x01,
        WriteAttributeNoResponse = 0x05,
        ConfigureReporting = 0x06,
        ConfigureReportingResponse = 0x07,
        ReadReportingConfiguration = 0x08,
        ReadReportingConfigurationResponse = 0x09,
        ReportAttributes = 0x0A,
        ReportAttributesAlarm = 0x8A,
        ClusterSpecificCommand = 0x50,
    }
}
