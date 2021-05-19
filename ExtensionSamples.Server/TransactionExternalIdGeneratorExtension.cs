using System;
using Storix.Model;
using Storix.Model.Transactions;

namespace ExtensionSamples.Server
{
    public class TransactionExternalIdGeneratorExtension : ITransactionExternalIdGenerator
    {
        private readonly ITransactionExternalIdGenerator _coreImplementation;

        public TransactionExternalIdGeneratorExtension(ITransactionExternalIdGenerator transactionExternalIdGenerator)
        {
            _coreImplementation = transactionExternalIdGenerator;
        }

        public string Generate(ICustomerOrder customerOrder)
        {
            var coreGeneratedString = _coreImplementation.Generate(customerOrder);

            return coreGeneratedString + customerOrder.POSTransactionID;
        }
    }
}
