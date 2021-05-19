using Storix.Contract.Base.Requests;

namespace ExtensionSamples.Server.Contract.Requests
{
    public class ExternalFuelDeliveryNotificationMessageRequest : Request
    {
        public int DestinationExternalBusinessUnitId { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
    }
}
