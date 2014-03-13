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
        bool ReadChannel(EGpioChannel channel);
        void WriteChannel(EGpioChannel channel, bool value);
    }
}
