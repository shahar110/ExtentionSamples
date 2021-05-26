using Storix.Contract.Base.Requests;
using System;

namespace ExtensionSamples.Server.Contract.Requests
{
    public class ExternalBusinessUnitManagementRequest : Request
    {
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public string ParentId { get; set; }
        public string Status { get; set; }
        public string StationType { get; set; }
        public string StationFormat { get; set; }
        public string CustomerId { get; set; }
        public string RecipientId { get; set; }
        public string ChannelId { get; set; }
        public string CashBookId { get; set; }
        public string ProfitCenterId { get; set; }
        public string CostCenterId { get; set; }
    }
}
