using ExtensionSamples.Server.DataAccess;
using Storix.Model.Builders;
namespace Storix.Server.Extension.DataAccess
{
    public class StorixExtensionContextOrgIdBuilder : IStorixTDMDBsContextOrgIdBuilder
    {
        private readonly Storix_PortalEntities _serverExtensionEntities;
        public StorixExtensionContextOrgIdBuilder(Storix_PortalEntities serverExtensionEntities)
        {
            _serverExtensionEntities = serverExtensionEntities;
        }
        public void SetOrganizationId(int organizationId)
        {
            _serverExtensionEntities.OrganizationId = organizationId;
        }
    }
}