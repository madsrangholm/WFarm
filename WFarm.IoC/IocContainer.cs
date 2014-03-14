using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using WFarm.Hardware;
using WFarm.Hardware.Gpio;
using WFarm.Logic.Interfaces;
using WFarm.Logic.Interfaces.Hardware;
using Component = Castle.MicroKernel.Registration.Component;


namespace WFarm.IoC
{
    public class IocContainer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Assume(container != null);
            container.Register(Component.For<IGpioHandler>().ImplementedBy<GpioHandler>().LifestyleTransient());
            container.Register(Component.For<IGpioChannel>().ImplementedBy<GpioChannel>().LifestyleTransient());
            container.Register(Component.For<IHardwareConfig>().ImplementedBy<HardwareConfig>().LifestyleSingleton());

            //container.AddFacility<TypedFactoryFacility>();
            //container.Register(Component.For<ICardFactory>().AsFactory());
            //container.Register(Component.For<ICard>().ImplementedBy<Card>().LifestyleTransient());
            //container.AddFacility<TypedFactoryFacility>();
            //container.Register(Component.For<IMyFirstFactory>().AsFactory());
            //container
            //    .Register(Component.For<IStartPageModel>().ImplementedBy<StartPageModel>().LifestyleTransient())
            //    .Register(Component.For<IStartPageViewModel>().ImplementedBy<StartPageViewModel>().LifestyleTransient())
            //    .Register(Component.For<IHeading>().ImplementedBy<Heading>().LifestyleTransient())
            //    .Register(Component.For<IShell>().ImplementedBy<Shell>().LifestyleTransient())
            //    .Register(Component.For<MainWindow>().LifestyleTransient());
        }
    }
}
