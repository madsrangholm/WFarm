using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using WFarm.Logic.Enums;
using WFarm.Logic.Interfaces;

namespace WFarm.Program
{
    class Program
    {
        private static IWindsorContainer _container;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            _container = new WindsorContainer();
            _container.Install(new IoC.IocContainer());
            new Program(_container.Resolve<IGpioHandler>());
        }

        public Program(IGpioHandler gpio)
        {
            gpio.SetupChannel(GpioChannel.Seven, GpioDirection.Output);
            for (var i = 0; i < 10; i++)
            {
                gpio.WriteChannel(GpioChannel.Seven, true);
                Thread.Sleep(400);
                gpio.WriteChannel(GpioChannel.Seven, false);
                Thread.Sleep(200);
            }
        }
    }
}
