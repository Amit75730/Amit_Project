using EdiClasses;
using log4net;

public static class IEASegment
{
    public static IEA ProcessIEA(string[] lineData, MsgData msgData, ILog log)
    {
        log.Info("Started parsing IEA segment");

        IEA iea = new IEA();

        if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1]))
        {
            iea.NumberOfIncludedFunctionalGroups = int.Parse(lineData[1].Trim());
        }
        if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2]))
        {
            iea.InterchangeControlNumber = int.Parse(lineData[2].Trim());
        }

        // msgData.IeaSegment = iea;

        log.Info("Finished parsing IEA segment");
        return iea;
    }
}
