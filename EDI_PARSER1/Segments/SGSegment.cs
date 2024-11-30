using EdiClasses;
using log4net;

public static class SGSegment
{
    public static SG ProcessSG(string[] lineData, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        SG sg = new SG();
        log.Info("Started parsing the SG segment data");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            sg.ShipmentStatusCode = lineData[1].Trim();
        }
        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            sg.StatusReasonCode = lineData[2].Trim();
        }
        if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3]))
        {
            sg.DispositionCode = lineData[3].Trim();
        }
        if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4]))
        {
            sg.Date = DateOnly.ParseExact(lineData[4].Trim(), "yyyyMMdd");
        }
        if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5]))
        {
            sg.Time = TimeOnly.ParseExact(lineData[5].Trim(), "HHmm");
        }
        if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6]))
        {
            sg.TimeCode = lineData[6].Trim();
        }

        if (sg.Date != default && sg.Time != default)
        {
            try
            {
                sg.DateTime = sg.Date.ToDateTime(sg.Time);
                log.Info($"Parsed SG segment DateTime: {sg.DateTime}");
            }
            catch (Exception ex)
            {
                log.Error($"Error parsing SG segment DateTime: {ex.Message}");
            }
        }
        else
        {
            log.Error("Error parsing SG segment DateTime: Date or Time is invalid.");
        }
        // s.SgSegment.Add(sg);
        // msgData.st_to_se.Add(s);
        // msgData.SgSegment.Add(sg);
        log.Info("Finished parsing SG segment");
        return sg;
    }
}
