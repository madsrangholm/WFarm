using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WFarm.Logic.Enums;
using WFarm.Logic.Interfaces;
using WFarm.Logic.Interfaces.Hardware;

namespace WFarm.Hardware.Gpio
{

    public class GpioHandler : IGpioHandler
    {
        private readonly IDictionary<EGpioChannel, IGpioChannel> _channels = new Dictionary<EGpioChannel, IGpioChannel>();
        private readonly IGpioChannel _emptyChannel;//copying from this object when adding new channels. Fancy way of letting IOC create the object.
        private readonly object _channelLock = new object();
        public GpioHandler(IGpioChannel emptyChannel)
        {
            _emptyChannel = emptyChannel;
        }

        public bool ReadChannel(EGpioChannel channel)
        {
            if (_channels.ContainsKey(channel)) return _channels[channel].Value;
            var chan = (IGpioChannel)_emptyChannel.Clone();
            chan.Setup(channel, EGpioDirection.Input);
            _channels.Add(channel, chan);
            return _channels[channel].Value;
        }

        public void WriteChannel(EGpioChannel channel, bool value)
        {
            lock (_channelLock)
            {
                if (!_channels.ContainsKey(channel))
                {
                    var chan = (IGpioChannel)_emptyChannel.Clone();
                    chan.Setup(channel, EGpioDirection.Output);
                    _channels.Add(channel, chan);
                }
            }
            
            _channels[channel].Value = value;
        }


    }
}
