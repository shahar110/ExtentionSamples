using System.Collections.Generic;
using System.Linq;
using Storix.POS.Model;
using Storix.POS.Model.Interfaces;

namespace ExtensionSamples.Pos
{
    public class ExtendedTab : ITab
    {
        public List<PumpTransactionDetail> SelectTransactions(List<PumpTransactionDetail> allTransactions)
        {
            return allTransactions.Where(x => x.Volume > 10m).ToList();
        }

        public EnumPumpViewMode PumpViewMode => EnumPumpViewMode.ActiveTrans;
    }
}
