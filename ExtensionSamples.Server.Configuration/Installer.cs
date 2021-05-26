using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExtensionSamples.Server.API.ManagementServices;
using Storix.API.Model;
using Storix.Model;
using Storix.Model.Builders;
using Storix.Server.Extension.DataAccess;

namespace ExtensionSamples.Server.Configuration
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IStorixService>()
                        .ImplementedBy<ExternalFuelDeliveryNotificationMessageManagementService>().Named(typeof(ExternalFuelDeliveryNotificationMessageManagementService).Name).LifeStyle.Transient);

            container.Register(Component.For<IStorixService>()
            .ImplementedBy<ExternalBusinessUnitManagementService>().Named(typeof(ExternalBusinessUnitManagementService).Name).LifeStyle.Transient);

            container.Register(Component.For<IRequestUnitOfWork>()
               .ImplementedBy<StorixExtensionUOW>().LifestyleTransient());

            container.Register(Component.For<IStorixTDMDBsContextOrgIdBuilder>()
                .ImplementedBy<StorixExtensionContextOrgIdBuilder>().LifestyleTransient());
        }
    }
}
