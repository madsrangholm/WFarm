using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

// Pi# Namespaces
using WFarm.Hardware.Gpio;
using WFarm.Hardware.Gpio.Entities;
using WFarm.Logic.Enums;
using WFarm.Logic.Interfaces;

namespace WFarm.Hardware
{
    public class GpioHandler : IGpioHandler
    {
        //public GpioHardware(bool testMode)
        //{
        //    LibGpio.Gpio.TestMode = testMode;
        //}

        public void SetupChannel(GpioChannel channel, GpioDirection direction)
        {
            LibGpio.Gpio.SetupChannel(TranslateChannel(channel), TranslateDirection(direction));
        }

        public bool ReadChannel(GpioChannel channel)
        {
            return LibGpio.Gpio.ReadValue(TranslateChannel(channel));
        }

        public void WriteChannel(GpioChannel channel, bool value)
        {
            LibGpio.Gpio.OutputValue(TranslateChannel(channel), value);
        }
        #region convert to library enums
        private RaspberryPinNumber TranslateChannel(GpioChannel channel)
        {
            switch (channel)
            {
                case GpioChannel.Zero: return RaspberryPinNumber.Zero;
                case GpioChannel.One: return RaspberryPinNumber.One;
                case GpioChannel.Two: return RaspberryPinNumber.Two;
                case GpioChannel.Three: return RaspberryPinNumber.Three;
                case GpioChannel.Four: return RaspberryPinNumber.Four;
                case GpioChannel.Five: return RaspberryPinNumber.Five;
                case GpioChannel.Six: return RaspberryPinNumber.Six;
                case GpioChannel.Seven:
                default: return RaspberryPinNumber.Seven;
            }
        }

        private Direction TranslateDirection(GpioDirection direction)
        {
            return direction == GpioDirection.Input
                ? Direction.Input
                : Direction.Output;
        }
        #endregion
    }
}
