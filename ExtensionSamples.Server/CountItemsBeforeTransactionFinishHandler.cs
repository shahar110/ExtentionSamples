using System;
using Storix.Model.DomainEvent;
using Storix.Model.DomainEvent.Contexts;

namespace ExtensionSamples.Server
{
    public class CountItemsBeforeTransactionFinishHandler : IDomainEventHandler<BeforeTransactionFinishedEventContext>
    {
        public bool IsRelevant(BeforeTransactionFinishedEventContext context, object sender)
        {
            return context.CustomerOrder.SaleLines.Count > 3;
        }

        public void Handle(BeforeTransactionFinishedEventContext context, object sender)
        {
            throw new Exception("Too many items in transaction!!");
        }
    }
}
