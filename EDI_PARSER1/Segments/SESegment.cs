
using EdiClasses;
using log4net;

public static class SESegment
{
    public static SE ProcessSE(string[] lineData, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        SE se = new SE();
        log.Info("Started parsing SE segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            se.NumberOfIncludedSegments = int.Parse(lineData[1].Trim());
        }

        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            se.TransactionSetControlNumber = int.Parse(lineData[2].Trim());
        }
        // s.SeSegment=se;
        //  msgData.st_to_se.Add(s);
        //msgData.SeSegment.Add(se);
        log.Info("Finished parsing SE segment");
        return se;
    }
}
