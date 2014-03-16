using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace WFarm.Test
{
    public class TestBase : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
        }
    }
}
