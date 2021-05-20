using System.Data;
using ExtensionSamples.Server.DataAccess;
using Storix.Common.EntityFramework;
using Storix.Model;


namespace ExtensionSamples.Server.API.ManagementServices
{
    public class StorixExtensionUOW : UnitOfWork, IRequestUnitOfWork
    {
        public StorixExtensionUOW(Storix_PortalEntities dbContext) : base(dbContext, IsolationLevel.ReadCommitted)
        {
        }
    }
}

