using Newtonsoft.Json;

namespace EdiWebAPI.Models
{
    public class DemurrageFees
    {
        public decimal FeesDue { get; set; }
        public decimal FeesPaid { get; set; }
    }

    public class RequiredFields
    {
        public string Id { get; set; }
        public string ContainerNumber { get; set; }
        public string TradeType { get; set; }
        public string Status { get; set; }
        public string Origin { get; set; }
        public int VesselCode { get; set; }
        public string VesselName { get; set; }
        public string FlightNumber { get; set; }
        public int TransactionSetControlNumber { get; set; }
        public DemurrageFees Demurrage_fees { get; set; }
    }
}