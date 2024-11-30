using EdiClasses;
using log4net;

public static class STSegment
{
    public static ST ProcessST(string[] linedata, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        ST st = new ST();
        log.Info("started parsing ST segment");
        if (linedata.Length > 1 && !string.IsNullOrWhiteSpace(linedata[1]))
        {
            st.TransactionSetIdentifierCode = int.Parse(linedata[1].Trim());
        }

        if (linedata.Length > 2 && !string.IsNullOrWhiteSpace(linedata[2]))
        {
            st.TransactionSetControlNumber = int.Parse(linedata[2].Trim());
        }
        //  s.StSegment = st;
        //msgData.St_.Add(s);
        //s.StSegment = st;
        //msgData.StSegment.Add(st);
        //msgData.StToSe.Add(s);
        log.Info("finished parsing ST segment");
        return st;

    }
}