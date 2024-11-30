using System;
using EdiClasses;
using log4net;

public static class N9Segment
{
    public static N9 ProcessN9(string[] linedata, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        N9 n9 = new N9();
        log.Info("started parsing N9 segment");
        if (linedata.Length > 1 && !string.IsNullOrWhiteSpace(linedata[1]))
        {
            n9.ReferenceIdentificationQualifier = linedata[1].Trim();
        }
        if (linedata.Length > 2 && !string.IsNullOrWhiteSpace(linedata[2]))
        {
            n9.ReferenceIdentification = linedata[2].Trim();
        }
        if (linedata.Length > 3 && !string.IsNullOrWhiteSpace(linedata[3]))
        {
            n9.FreeFormIdentification = linedata[3].Trim();
        }
        if (linedata.Length > 4 && !string.IsNullOrWhiteSpace(linedata[4]))
        {
            string dateString = linedata[4].Trim();
            log.Info($"Parsing Date: {dateString}");
            n9.Date = DateOnly.ParseExact(dateString, "yyyyMMdd");
        }
        if (linedata.Length > 5 && !string.IsNullOrWhiteSpace(linedata[5]))
        {
            n9.Time = TimeOnly.ParseExact(linedata[5].Trim(), "HHmm");
        }
        if (linedata.Length > 6 && !string.IsNullOrWhiteSpace(linedata[6]))
        {
            n9.TimeCode = linedata[6].Trim();
        }
        if (linedata.Length > 7 && !string.IsNullOrWhiteSpace(linedata[7]))
        {
            n9.ReferenceIdentificationQualifier1 = linedata[7].Trim();
        }
        if (linedata.Length > 8 && !string.IsNullOrWhiteSpace(linedata[8]))
        {
            n9.ReferenceIdentification1 = linedata[8].Trim();
        }
        if (linedata.Length > 9 && !string.IsNullOrWhiteSpace(linedata[9]))
        {
            n9.FreeFormIdentificationQualifier1 = linedata[9].Trim();
        }
        if (linedata.Length > 10 && !string.IsNullOrWhiteSpace(linedata[10]))
        {
            n9.ReferenceIdentification2 = linedata[10].Trim();
        }
        if (linedata.Length > 11 && !string.IsNullOrWhiteSpace(linedata[11]))
        {
            n9.FreeFormIdentificationQualifier2 = linedata[11].Trim();
        }
        if (linedata.Length > 12 && !string.IsNullOrWhiteSpace(linedata[12]))
        {
            n9.ReferenceIdentification3 = linedata[12].Trim();
        }
        if (n9.Date != default && n9.Time != default)
        {
            try
            {
                n9.DateTime = n9.Date.ToDateTime(n9.Time);
                log.Info($"Parsed N9 segment DateTime: {n9.DateTime}");
            }
            catch (Exception ex)
            {
                log.Error($"Error parsing N9 segment DateTime: {ex.Message}");
            }
        }
        return n9;

        //msgData.N9Segment.Add(n9);
    }
}

