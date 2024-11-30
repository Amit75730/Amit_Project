using EdiClasses;
using log4net;

public static class GESegment
{
    public static GE ProcessGE(string[] lineData, MsgData msgData, ILog log)
    {
        GE ge = new GE();
        log.Info("Started parsing GE segment");

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            ge.NumberOfTransactionSetsIncluded = int.Parse(lineData[1].Trim());
        }

        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            ge.GroupControlNumber = int.Parse(lineData[2].Trim());
        }

        // msgData.GeSegment = ge;

        log.Info("Finished parsing GE segment");
        return ge;
    }

}
