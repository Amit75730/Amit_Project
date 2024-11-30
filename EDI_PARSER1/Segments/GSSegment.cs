using EdiClasses;
using log4net;

public static class GSSegment
{
    public static GS ProcessGS(string[] lineData, MsgData msgData, ILog log)
    {
        GS gs = new GS();
        log.Info("Started parsing GS segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            gs.FunctionalIdentifierCode = lineData[1].Trim();
        }
        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            gs.FunctionalSendersCode = lineData[2].Trim();
        }
        if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3]))
        {
            gs.FunctionalReceiversCode = lineData[3].Trim();
        }
        if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4]))
        {
            gs.GroupDate = DateOnly.ParseExact(lineData[4].Trim(), "yyyyMMdd");
        }
        if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5]))
        {
            gs.GroupTime = TimeOnly.ParseExact(lineData[5].Trim(), "HHmm");
        }
        if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6]))
        {
            gs.GroupControlNumber = int.Parse(lineData[6].Trim());
        }

        if (lineData.Length > 7 && !string.IsNullOrWhiteSpace(lineData[7]))
        {
            gs.ResponsibleAgencyCode = lineData[7].Trim();
        }
        if (lineData.Length > 8 && !string.IsNullOrWhiteSpace(lineData[8]))
        {
            gs.Version = int.Parse(lineData[8].Trim());
        }

        if (gs.GroupDate != default && gs.GroupTime != default)
        {
            try
            {
                gs.DateTime = gs.GroupDate.ToDateTime(gs.GroupTime);
                log.Info($"Parsed GS segment DateTime: {gs.DateTime}");
            }
            catch (Exception ex)
            {
                log.Error($"Error parsing GS segment DateTime: {ex.Message}");
            }
        }

        // msgData.GsSegment = gs;
        log.Info("Finished parsing GS segment");
        return gs;
    }
}
