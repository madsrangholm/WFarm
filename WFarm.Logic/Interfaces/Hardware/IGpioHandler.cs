using WFarm.Logic.Enums;

namespace WFarm.Logic.Interfaces.Hardware
{
    public interface IGpioHandler
    {
        bool ReadChannel(EGpioChannel channel);
        void WriteChannel(EGpioChannel channel, bool value);
    }
}
