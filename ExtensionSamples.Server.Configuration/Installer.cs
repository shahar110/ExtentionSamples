using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExtensionSamples.Server.API.ManagementServices;
using Storix.API.Model;

namespace ExtensionSamples.Server.Configuration
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IStorixService>()
                        .ImplementedBy<ExternalFuelDeliveryNotificationMessageManagementService>().Named(typeof(ExternalFuelDeliveryNotificationMessageManagementService).Name));

            container.Register(Component.For<IStorixService>()
            .ImplementedBy<ExternalBusinessUnitManagementService>().Named(typeof(ExternalBusinessUnitManagementService).Name));
        }
    }
}
