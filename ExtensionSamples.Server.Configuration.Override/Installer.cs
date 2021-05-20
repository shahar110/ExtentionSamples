using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExtensionSamples.Server.DataAccess;
using ExtensionSamples.Server.DataAccess.Repositories;
using Storix.Model.Repositories;

namespace ExtensionSamples.Server.Configuration.Override
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Storix_PortalEntities>()
                                    .LifestylePerWcfOperation());

            container.Register(Component.For<IBusinessUnitRepository>()
                                    .ImplementedBy<ExtensionBusinessUnitRepository>()
                                    .LifestyleTransient());
        }
    }
}
