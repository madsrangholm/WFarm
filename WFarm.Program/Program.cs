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
using WFarm.Logic.Interfaces.Hardware;

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
            for (var i = 0; i < 10; i++)
            {
                gpio.WriteChannel(EGpioChannel.Seven, true);
                Thread.Sleep(400);
                gpio.WriteChannel(EGpioChannel.Seven, false);
                Thread.Sleep(200);
            }
        }
    }
}
