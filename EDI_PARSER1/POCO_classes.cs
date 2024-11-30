using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EdiClasses
{
    public class MsgData
    {
        [NotMapped]
        public ISA IsaSegment { get; set; }
        [NotMapped]
        public GS GsSegment { get; set; }
        public List<St_to_SeSegment> st_to_se { get; set; } = new List<St_to_SeSegment>();
        [NotMapped]
        public GE GeSegment { get; set; }
        [NotMapped]
        public IEA IeaSegment { get; set; }
    }
    public class St_to_SeSegment
    {
        public string ContainerNumber { get; set; }
        public ST StSegment { get; set; }
        public List<B4> B4Segment { get; set; } = new List<B4>();
        public List<N9> N9Segment { get; set; } = new List<N9>();
        public Q2 Q2Segment { get; set; }
        public List<SG> SgSegment { get; set; } = new List<SG>();
        public List<R4> R4Segment { get; set; } = new List<R4>();
        public SE SeSegment { get; set; }
    }


    public class ISA
    {
        public string AuthorizationInformationQualifier { get; set; }
        public string AuthorizationInformation { get; set; }
        public string SecurityInformationQualifier { get; set; }
        public string SecurityInformation { get; set; }
        public string InterchangeIdQualifierSender { get; set; }
        public string InterchangeSenderIdQualifier { get; set; }
        public string InterchangeIdQualifier { get; set; }
        public string InterchangeReceiverId { get; set; }

        [JsonIgnore]
        public DateOnly Date { get; set; }

        [JsonIgnore]
        public TimeOnly Time { get; set; }

        public string InterchangeControlStandardIdCode { get; set; }
        public int InterchangeVersion { get; set; }
        public int InterchangeControlNumber { get; set; }
        public int AcknowledgementRequested { get; set; }
        public string TestIndicator { get; set; }
        public string SubElementSeparator { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class GS
    {


        // public string id { get; set; }= Guid.NewGuid().ToString();
        // public string MsgDataid { get; set; }  // Foreign key to MsgData
        // public MsgData MsgData { get; set; }  // Navigation property to MsgData


        public string FunctionalIdentifierCode { get; set; }
        public string FunctionalSendersCode { get; set; }
        public string FunctionalReceiversCode { get; set; }

        [JsonIgnore]
        public DateOnly GroupDate { get; set; }

        [JsonIgnore]
        public TimeOnly GroupTime { get; set; }

        public int GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public int Version { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class ST
    {
        public int TransactionSetIdentifierCode { get; set; }
        public int TransactionSetControlNumber { get; set; }
    }

    public class B4
    {
        public string SpecialHandlingCode { get; set; }
        public string InquiryRequestNumber { get; set; }
        public string ShipmentStatusCode { get; set; }

        [JsonIgnore]
        public DateOnly ReleaseDate { get; set; }

        [JsonIgnore]
        public TimeOnly ReleaseTime { get; set; }

        public string StatusLocation { get; set; }

        [JsonIgnore]
        public string EquipmentInitial { get; set; }

        [JsonIgnore]
        public string EquipmentNumber { get; set; }

        public string EquipmentStatusCode { get; set; }
        public string EquipmentType { get; set; }
        public string LocationIdentifier { get; set; }
        public string LocationQualifier { get; set; }
        public DateTime? DateTime { get; set; }
        public string ContainerNumber { get; set; }
    }

    public class N9
    {
        public string? ReferenceIdentificationQualifier { get; set; }
        public string? ReferenceIdentification { get; set; }
        public string? FreeFormIdentification { get; set; }

        [JsonIgnore]
        public DateOnly Date { get; set; }
        [JsonIgnore]
        public TimeOnly Time { get; set; }

        public string? TimeCode { get; set; }
        public string? ReferenceIdentificationQualifier1 { get; set; }
        public string? ReferenceIdentification1 { get; set; }
        public string? FreeFormIdentificationQualifier1 { get; set; }
        public string? ReferenceIdentification2 { get; set; }
        public string? FreeFormIdentificationQualifier2 { get; set; }
        public string? ReferenceIdentification3 { get; set; }
        public DateTime? DateTime { get; set; }
    }
    public class Q2
    {
        public int VesselCode { get; set; }
        public string CountryCode { get; set; }
        public string Date { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string LadingQuantity { get; set; }
        public string Weight { get; set; }
        public string WeightQualifier { get; set; }
        public string FlightNumber { get; set; }
        public string ReferenceIdentificationQualifier { get; set; }
        public string ReferenceIdentification { get; set; }
        public string VesselCodeQualifier { get; set; }
        public string VesselName { get; set; }
        public string Volume { get; set; }
        public string VolumeUnitQualifier { get; set; }
        public string WeightUnitCode { get; set; }
    }

    public class SG
    {
        public string ShipmentStatusCode { get; set; }
        public string StatusReasonCode { get; set; }
        public string DispositionCode { get; set; }

        [JsonIgnore]
        public DateOnly Date { get; set; }

        [JsonIgnore]
        public TimeOnly Time { get; set; }

        public string TimeCode { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class R4
    {
        public string PortOrTerminalFunctionCode { get; set; }
        public string LocationQualifier { get; set; }
        public int LocationIdentifier { get; set; }
        public string PortName { get; set; }
        public string CountryCode { get; set; }
        public string TerminalName { get; set; }
        public string PierNumber { get; set; }
        public string StateOrProvinceCode { get; set; }
    }

    public class SE
    {
        public int NumberOfIncludedSegments { get; set; }
        public int TransactionSetControlNumber { get; set; }
    }

    public class GE
    {
        public int NumberOfTransactionSetsIncluded { get; set; }
        public int GroupControlNumber { get; set; }
    }

    public class IEA
    {
        public int NumberOfIncludedFunctionalGroups { get; set; }
        public int InterchangeControlNumber { get; set; }
    }
    public class Required_Fields
    {

        public string ContainerNumber { get; set; }
        public string TradeType { get; set; }
        public string Origin { get; set; }
        // public string EquipmentInitial{get;set;}
        // public string EquipmentNumber{get;set;}
        public int VesselCode { get; set; }
        public string VesselName { get; set; }
        public string FlightNumber { get; set; }
        public string TransactionSetControlNumber { get; set; }
    }
}
