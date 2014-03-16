using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Core;
using NUnit.Framework;
using WFarm.Hardware;
using WFarm.Hardware.Gpio;
using WFarm.Logic.Enums;
using WFarm.Logic.Exceptions;
using WFarm.Logic.Interfaces;
using WFarm.Logic.Interfaces.Hardware;

namespace WFarm.Test.Integration
{
    [TestFixture]
    public class GpioIntegrationTest : TestBase
    {
        private readonly IWindsorContainer _container;
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        public class Container : IWindsorInstaller
        {
            public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
            {
                Contract.Assume(container != null);
                container.Register(Component.For<IGpioHandler>().ImplementedBy<GpioHandler>().LifestyleTransient());
                container.Register(Component.For<IGpioChannel>().ImplementedBy<GpioChannel>().LifestyleTransient());
                container.Register(Component.For<IHardwareConfig>().ImplementedBy<HardwareConfig>().LifestyleTransient());
            }
        }
        public GpioIntegrationTest()
        {
            _container = new WindsorContainer();
            _container.Install(new Container());
        }
        [Test]
        public void TestReadChannels()
        {
            var handler =_container.Resolve<IGpioHandler>();
            foreach (var channel in GetValues<EGpioChannel>())
            {
                handler.ReadChannel(channel);
            }
        }
        [Test]
        public void TestWriteChannels()
        {
            var handler = _container.Resolve<IGpioHandler>();
            foreach (var channel in GetValues<EGpioChannel>())
            {
                handler.WriteChannel(channel, false);
                handler.WriteChannel(channel, true);
            }
        }
        [Test]
        [ExpectedException(typeof(GpioException))]
        public void TestReadWriteChannels()
        {
            var handler = _container.Resolve<IGpioHandler>();
            foreach (var channel in GetValues<EGpioChannel>())
            {
                handler.WriteChannel(channel, false);
                handler.ReadChannel(channel);
            }
        }
    }
}
