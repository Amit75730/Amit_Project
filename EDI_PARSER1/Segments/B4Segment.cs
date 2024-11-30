using EdiClasses;
using log4net;

public static class B4Segment
{
    public static B4 ProcessB4(string[] lineData, MsgData msgData, St_to_SeSegment s, ILog log)
    {
        try
        {
            B4 b4 = new B4();
            log.Info("Started parsing B4 segment");

            if (lineData.Length > 1 && !string.IsNullOrWhiteSpace(lineData[1])) b4.SpecialHandlingCode = lineData[1].Trim();
            if (lineData.Length > 2 && !string.IsNullOrWhiteSpace(lineData[2])) b4.InquiryRequestNumber = lineData[2].Trim();
            if (lineData.Length > 3 && !string.IsNullOrWhiteSpace(lineData[3])) b4.ShipmentStatusCode = lineData[3].Trim();
            if (lineData.Length > 4 && !string.IsNullOrWhiteSpace(lineData[4])) b4.ReleaseDate = DateOnly.ParseExact(lineData[4].Trim(), "yyyyMMdd");
            if (lineData.Length > 5 && !string.IsNullOrWhiteSpace(lineData[5])) b4.ReleaseTime = TimeOnly.ParseExact(lineData[5].Trim(), "HHmm");
            if (lineData.Length > 6 && !string.IsNullOrWhiteSpace(lineData[6])) b4.StatusLocation = lineData[6].Trim();
            if (lineData.Length > 7 && !string.IsNullOrWhiteSpace(lineData[7])) b4.EquipmentInitial = lineData[7].Trim();
            if (lineData.Length > 8 && !string.IsNullOrWhiteSpace(lineData[8])) b4.EquipmentNumber = lineData[8].Trim();
            if (lineData.Length > 9 && !string.IsNullOrWhiteSpace(lineData[9])) b4.EquipmentStatusCode = lineData[9].Trim();
            if (lineData.Length > 10 && !string.IsNullOrWhiteSpace(lineData[10])) b4.EquipmentType = lineData[10].Trim();
            if (lineData.Length > 11 && !string.IsNullOrWhiteSpace(lineData[11])) b4.LocationIdentifier = lineData[11].Trim();
            if (lineData.Length > 12 && !string.IsNullOrWhiteSpace(lineData[12])) b4.LocationQualifier = lineData[12].Trim();

            if (b4.ReleaseDate != default && b4.ReleaseTime != default)
            {
                try
                {
                    b4.DateTime = b4.ReleaseDate.ToDateTime(b4.ReleaseTime);
                    log.Info($"Parsed B4 segment DateTime: {b4.DateTime}");
                }
                catch (Exception ex)
                {
                    log.Error($"Error parsing B4 segment DateTime: {ex.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(b4.EquipmentInitial) && !string.IsNullOrWhiteSpace(b4.EquipmentNumber))
            {
                string containerNumber = string.Concat(b4.EquipmentInitial, b4.EquipmentNumber);

                if (containerNumber.Length != 11)
                {
                    log.Error("The ContainerNumber must have exactly 11 characters.");
                    throw new FormatException("The ContainerNumber must have exactly 11 characters.");
                }

                string firstFour = containerNumber.Substring(0, 4);
                if (!firstFour.All(char.IsLetter))
                {
                    log.Error("The first 4 characters of ContainerNumber must be letters.");
                    throw new FormatException("The first 4 characters of ContainerNumber must be letters.");
                }

                string lastSeven = containerNumber.Substring(4, 7);
                if (!lastSeven.All(char.IsDigit))
                {
                    log.Error("The last 7 characters of ContainerNumber must be digits.");
                    throw new FormatException("The last 7 characters of ContainerNumber must be digits.");
                }

                b4.ContainerNumber = containerNumber;
            }
            else
            {
                log.Error("EquipmentInitial and EquipmentNumber must not be empty.");
                throw new FormatException("EquipmentInitial and EquipmentNumber must not be empty.");
            }
            //s.B4Segment.Add(b4);
            return b4;
            //msgData.B4Segment.Add(b4);
        }
        catch (Exception ex)
        {
            log.Error($"Error while processing B4 segment: {ex.Message}");
            throw new Exception($"Error while processing B4 segment: {ex.Message}");
        }
    }
}
