using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFarm.Logic.Enums;

namespace WFarm.Logic.Interfaces.Hardware
{
    public interface IHardwareConfig
    {
        ESystemPlatform SystemPlatform { get; }
    }
}
