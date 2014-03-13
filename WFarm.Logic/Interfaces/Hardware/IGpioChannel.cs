using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFarm.Logic.Enums;

namespace WFarm.Logic.Interfaces
{
    public interface IGpioChannel : ICloneable
    {
        void Setup(EGpioChannel channel, EGpioDirection direction);
        EGpioChannel Channel { get; }
        EGpioDirection Direction { get; }
        bool Value { get; set; }
    }
}
