using EdiClasses;
using log4net;

public static class Q2Segment
{
    public static Q2 ProcessQ2(string[] lineData, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        Q2 q2 = new Q2();
        log.Info("Started parsing Q2 segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            q2.VesselCode = int.Parse(lineData[1].Trim());
        }
        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            q2.CountryCode = lineData[2].Trim();
        }
        if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3]))
        {
            q2.Date = lineData[3].Trim();
        }
        if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4]))
        {
            q2.Date1 = lineData[4].Trim();
        }
        if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5]))
        {
            q2.Date2 = lineData[5].Trim();
        }
        if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6]))
        {
            q2.LadingQuantity = lineData[6].Trim();
        }
        if (lineData.Length > 7 && !string.IsNullOrWhiteSpace(lineData[7]))
        {
            q2.Weight = lineData[7].Trim();
        }
        if (lineData.Length > 8 && !string.IsNullOrWhiteSpace(lineData[8]))
        {
            q2.WeightQualifier = lineData[8].Trim();
        }
        if (lineData.Length > 9 && !string.IsNullOrWhiteSpace(lineData[9]))
        {
            q2.FlightNumber = lineData[9].Trim();
        }
        if (lineData.Length > 10 && !string.IsNullOrWhiteSpace(lineData[10]))
        {
            q2.ReferenceIdentificationQualifier = lineData[10].Trim();
        }
        if (lineData.Length > 11 && !string.IsNullOrWhiteSpace(lineData[11]))
        {
            q2.ReferenceIdentification = lineData[11].Trim();
        }
        if (lineData.Length > 12 && !string.IsNullOrWhiteSpace(lineData[12]))
        {
            q2.VesselCodeQualifier = lineData[12].Trim();
        }
        if (lineData.Length > 13 && !string.IsNullOrWhiteSpace(lineData[13]))
        {
            q2.VesselName = lineData[13].Trim();
        }
        if (lineData.Length > 14 && !string.IsNullOrWhiteSpace(lineData[14]))
        {
            q2.Volume = lineData[14].Trim();
        }
        if (lineData.Length > 15 && !string.IsNullOrWhiteSpace(lineData[15]))
        {
            q2.VolumeUnitQualifier = lineData[15].Trim();
        }
        if (lineData.Length > 16 && !string.IsNullOrWhiteSpace(lineData[16]))
        {
            q2.WeightUnitCode = lineData[16].Trim();
        }
        // msgData.Q2Segment.Add(q2);
        return q2;
    }
}
