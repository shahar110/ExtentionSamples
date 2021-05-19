using Storix.Model.Entities.Lines;
using Storix.Model.Exceptions.StopWithoutRollbackExceptions.ItemAddRestriction;
using Storix.Model.Restrictions;
using Storix.Model.Transactions;

namespace ExtensionSamples.Server
{
    public class ExpensiveItemRestriction : IBeforeLineAddRestriction<ISaleLine>
    {
        public void Check(ICustomerOrder customerOrder, ISaleLine line)
        {
            if (line.CatalogPrice.Money > 10m)
                throw new AgeNotValidRestrictionException();
        }

        public int Priority { get; }
    }
}
