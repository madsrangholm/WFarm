using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFarm.Logic.Enums;
using WFarm.Logic.Interfaces.Hardware;

namespace WFarm.Hardware
{
    public class HardwareConfig : IHardwareConfig
    {
        public ESystemPlatform SystemPlatform
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.WinCE:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                        return ESystemPlatform.Windows;
                    case PlatformID.MacOSX:
                        return ESystemPlatform.Mac;
                    default:
                        return ESystemPlatform.Unix;
                }
            }
        }
    }
}
