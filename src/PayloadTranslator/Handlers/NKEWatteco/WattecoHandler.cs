using System;
using PayloadTranslator.Attributes;
using PayloadTranslator.Enums;
using PayloadTranslator.Entities;
using PayloadTranslator.Handlers.NKEWatteco.Helpers;
using Data.Enums;

namespace PayloadTranslator.Handlers.NKEWatteco
{
    [Sensor(DeviceTypes.SMARTPLUG)]
    public class WattecoHandler : Handler, IHandler
    {
        public static readonly string[] SupportedDeviceTypeNames = { "SMARTPLUG", "INTENSO" };

        public override PayloadResponse HandlePayload(PayloadRequest request)
        {
            var response = new PayloadResponse(request);

            try
            {
                var bytes = HexHelper.HexToBytes(request.Data);
                var result = Decode(bytes, (int)request.Payload.port);

                if (result != null)
                {
                    var powerOn = result.Data == 0 ? 0 : 1;
                    response.Measurements.Add(MeasumrentType.power_on.ToString(), powerOn);
                    response.Measurements.Add(MeasumrentType.ampere_amp.ToString(), result.Data);

                    if (result.SimpleMetering != null)
                    {
                        response.Measurements.Add(MeasumrentType.active_energy_wh.ToString(), result.SimpleMetering.ActiveEnergy);
                        response.Measurements.Add(MeasumrentType.reactive_energy_varh.ToString(), result.SimpleMetering.ReActiveEnergy);
                        response.Measurements.Add(MeasumrentType.active_power_w.ToString(), result.SimpleMetering.ActivePower);
                        response.Measurements.Add(MeasumrentType.reactive_power_var.ToString(), result.SimpleMetering.ReActivePower);
                        response.Measurements.Add(MeasumrentType.count_samples.ToString(), result.SimpleMetering.NumberOfSamples);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(WattecoHandler)} failed", ex);
            }

            return response;
        }

        private static SlotDescriptor ParseSlotDescriptors(byte[] bytes)
        {
            var criterialByte = bytes[12];

            var criterialIndex = criterialByte & 7;
            var mode = (Mode)((criterialByte >> 3) & 3);
            var onFall = Convert.ToBoolean((criterialByte >> 5) & 1);
            var onExceed = Convert.ToBoolean((criterialByte >> 6) & 1);
            var isAlarm = Convert.ToBoolean((criterialByte >> 7) & 1);

            var slotDescriptor = new SlotDescriptor
            {
                CriteriaSlotDescriptor = new Criteriaslotdescriptor
                {
                    Alarm = isAlarm,
                    OnExceed = onExceed,
                    OnFall = onFall,
                    Mode = mode.ToString(),
                    CriterionIndex = criterialIndex,
                },
            };

            return slotDescriptor;
        }

        private static Reportparameters ParseReportParameters(byte[] bytes)
        {
            var reportByte = bytes[11];

            var isBatch = Convert.ToBoolean(reportByte & 1);
            var noHeaderPort = Convert.ToBoolean((reportByte >> 1) & 1);
            var secured = Convert.ToBoolean((reportByte >> 2) & 1);
            var securedIfAlarm = Convert.ToBoolean((reportByte >> 3) & 1);
            var causeRequest = (CauseRequest)((reportByte >> 4) & 3);
            var reserved = (reportByte >> 6) & 1;

            var reportParameters = new Reportparameters
            {
                Batch = isBatch,
                Secured = secured,
                SecuredIfAlarm = securedIfAlarm,
                NoHeaderPort = noHeaderPort,
                Reserved = Convert.ToInt32(reserved),
                CauseRequest = causeRequest.ToString(),
            };
            return reportParameters;
        }

        private float Bytes2Float32(int bytes)
        {
            var fb = Convert.ToUInt32(bytes);
            return BitConverter.ToSingle(BitConverter.GetBytes((int)fb), 0);
        }

        private Result Decode(byte[] bytes, int port)
        {
            var result = new Result();
            if (port == 125)
            {
                //// We need at least the first 11 bytes to determine the value
                if (bytes.Length <= 11)
                {
                    return null;
                }

                var endpoint = bytes[0] & 1;
                if (endpoint != 0)
                {
                    var command = (Command)bytes[1];
                    var cluster = (Cluster)((bytes[2] * 256) + bytes[3]);

                    if (command == Command.ReportAttributes || command == Command.ReportAttributesAlarm || command == Command.ReadAttributeResponse)
                    {
                        var attributeId = (bytes[4] * 256) + bytes[5];
                        var type = bytes[6];

                        var value = -1f;
                        if ((cluster == Cluster.AnalogInput) && (attributeId == 0x0055))
                        {
                            var startIndex = 7;
                            value = Bytes2Float32((bytes[startIndex] * 256 * 256 * 256) + (bytes[startIndex + 1] * 256 * 256) + (bytes[startIndex + 2] * 256) + bytes[startIndex + 3]);
                        }

                        if (cluster == Cluster.SimpleMetering)
                        {
                            var simpleMetering = new SimpleMetering();
                            var startIndex = 8;

                            // summation of 24bit
                            simpleMetering.ActiveEnergy = Convert.ToInt32(bytes[startIndex] + bytes[startIndex + 1] + bytes[startIndex + 2]);
                            simpleMetering.ReActiveEnergy = Convert.ToInt32(bytes[startIndex + 3] + bytes[startIndex + 4] + bytes[startIndex + 5]);
                            simpleMetering.NumberOfSamples = Convert.ToInt32(bytes[startIndex + 6] + bytes[startIndex + 7]);

                            // 16bit
                            var activePowerArray = new byte[] { bytes[startIndex + 8], bytes[startIndex + 9] };
                            var reactivePowerArray = new byte[] { bytes[startIndex + 10], bytes[startIndex + 11] };

                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(activePowerArray);
                                Array.Reverse(reactivePowerArray);
                            }

                            simpleMetering.ActivePower = BitConverter.ToInt16(activePowerArray, 0);
                            simpleMetering.ReActivePower = BitConverter.ToInt16(reactivePowerArray, 0);

                            result.SimpleMetering = simpleMetering;
                        }

                        result.ClusterID = cluster.ToString();
                        result.CommandID = command.ToString();
                        result.Data = value;
                        result.EndPoint = endpoint;

                        if (bytes.Length < 12)
                        {
                            return result;
                        }

                        var reportParameters = ParseReportParameters(bytes);

                        var causes = new Cause[]
                        {
                            new Cause
                            {
                                ReportParameters = reportParameters,
                            },
                        };

                        result.Cause = causes;

                        if (bytes.Length < 13)
                        {
                            return result;
                        }

                        var slotDescriptors = ParseSlotDescriptors(bytes);
                        result.Cause[0].SlotDescriptors = new SlotDescriptor[]
                        {
                            slotDescriptors,
                        };

                        return result;
                    }
                }
            }

            return result;
        }
    }
}
