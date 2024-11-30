using System.Runtime.CompilerServices;
using EdiClasses;
using log4net;

public static class ISASegment
{
    public static ISA ProcessISA(string[] lineData, MsgData msgData, ILog log)
    {
        ISA isa = new ISA();
        log.Info("Started parsing ISA segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
            isa.AuthorizationInformationQualifier = lineData[1].Trim();

        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
            isa.AuthorizationInformation = lineData[2].Trim();

        if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3]))
            isa.SecurityInformationQualifier = lineData[3].Trim();

        if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4]))
            isa.SecurityInformation = lineData[4].Trim();

        if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5]))
            isa.InterchangeIdQualifierSender = lineData[5].Trim();

        if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6]))
            isa.InterchangeSenderIdQualifier = lineData[6].Trim();

        if (lineData.Length > 7 && !string.IsNullOrWhiteSpace(lineData[7]))
            isa.InterchangeIdQualifier = lineData[7].Trim();

        if (lineData.Length > 8 && !string.IsNullOrWhiteSpace(lineData[8]))
            isa.InterchangeReceiverId = lineData[8].Trim();

        if (lineData.Length > 9 && !string.IsNullOrWhiteSpace(lineData[9]))
            isa.Date = DateOnly.ParseExact(lineData[9].Trim(), "yyMMdd");

        if (lineData.Length > 10 && !string.IsNullOrWhiteSpace(lineData[10]))
            isa.Time = TimeOnly.ParseExact(lineData[10].Trim(), "HHmm");

        if (lineData.Length > 11 && !string.IsNullOrWhiteSpace(lineData[11]))
            isa.InterchangeControlStandardIdCode = lineData[11].Trim();

        if (lineData.Length > 12 && !string.IsNullOrWhiteSpace(lineData[12]))
            isa.InterchangeVersion = int.Parse(lineData[12].Trim());

        if (lineData.Length > 13 && !string.IsNullOrWhiteSpace(lineData[13]))
            isa.InterchangeControlNumber = int.Parse(lineData[13].Trim());

        if (lineData.Length > 14 && !string.IsNullOrWhiteSpace(lineData[14]))
            isa.AcknowledgementRequested = int.Parse(lineData[14].Trim());

        if (lineData.Length > 15 && !string.IsNullOrWhiteSpace(lineData[15]))
            isa.TestIndicator = lineData[15].Trim();

        if (lineData.Length > 16 && !string.IsNullOrWhiteSpace(lineData[16]))
            isa.SubElementSeparator = lineData[16].Trim();

        // Combine DateOnly and TimeOnly into DateTime
        if (isa.Date != default && isa.Time != default)
        {
            try
            {
                isa.DateTime = isa.Date.ToDateTime(isa.Time); //2024-09-23T23:46:00
                log.Info($"Parsed ISA segment DateTime: {isa.DateTime}");
            }
            catch (Exception ex)
            {
                log.Error($"Error combining ISA Date and Time: {ex.Message}");
            }
        }
        else
        {
            log.Warn("ISA Date or Time is invalid. Skipping DateTime parsing.");
        }

        //  msgData.IsaSegment = isa;
        log.Info("ISA segment parsed successfully.");
        return isa;
    }
}
