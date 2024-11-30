using EdiClasses;
using log4net;

public static class R4Segment
{
    public static R4 ProcessR4(string[] lineData, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        R4 r4 = new R4();
        log.Info("Started parsing R4 segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            r4.PortOrTerminalFunctionCode = lineData[1].Trim();
        }
        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            r4.LocationQualifier = lineData[2].Trim();
        }
        if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3]))
        {
            r4.LocationIdentifier = int.Parse(lineData[3].Trim());
        }
        if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4]))
        {
            r4.PortName = lineData[4].Trim();
        }
        if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5]))
        {
            r4.CountryCode = lineData[5].Trim();
        }
        if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6]))
        {
            r4.TerminalName = lineData[6].Trim();
        }
        if (lineData.Length > 7 && !string.IsNullOrWhiteSpace(lineData[7]))
        {
            r4.PierNumber = lineData[7].Trim();
        }
        if (lineData.Length > 8 && !string.IsNullOrWhiteSpace(lineData[8]))
        {
            r4.StateOrProvinceCode = lineData[8].Trim();
        }
        // s.R4Segment.Add(r4);
        return r4;
        // msgData.st_to_se.Add(s);
        // msgData.R4Segment.Add(r4);
    }
}
