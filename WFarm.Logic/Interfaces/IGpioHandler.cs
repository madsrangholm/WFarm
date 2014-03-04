using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFarm.Logic.Enums;

namespace WFarm.Logic.Interfaces
{
    public interface IGpioHandler
    {
        void SetupChannel(GpioChannel channel, GpioDirection direction);
        bool ReadChannel(GpioChannel channel);
        void WriteChannel(GpioChannel channel, bool value);
    }
}
