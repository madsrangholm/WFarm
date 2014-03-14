using System;
using System.IO;
using WFarm.Logic.Enums;
using WFarm.Logic.Exceptions;
using WFarm.Logic.Interfaces;
using WFarm.Logic.Interfaces.Hardware;

namespace WFarm.Hardware.Gpio
{
    public class GpioChannel : IGpioChannel
    {
        /// <summary>
        /// Used for enforcing that only one thread can access files of a specific channel at once to prevent raceconditions.
        /// </summary>
        private readonly object _chanLock = new object();

        private string GpioPath
        {
            get
            {
                switch (_config.SystemPlatform)
                {
                    case ESystemPlatform.Windows:
                        return @"C:\RasPiGpioTest";
                    case ESystemPlatform.Mac:
                        return @"/tmp/RasPiGpioTest";
                    default:
                        return @"/sys/class/gpio";
                }
            }
        }

        public EGpioDirection Direction { get; set; }
        public EGpioChannel Channel { get; set; }
        private bool _isInitialized;
        private string GpioDir
        {
            get { return Path.Combine(GpioPath, string.Format("gpio{0}", (int) Channel)); }
        }

        private readonly IHardwareConfig _config;
        public GpioChannel(IHardwareConfig config)
        {
            _config = config;
        }

        public void Setup(EGpioChannel channel, EGpioDirection direction)
        {
            Channel = channel;
            Direction = direction;
            Initialize();
            _isInitialized = true;
        }
        public bool Value
        {
            get
            {
                if(!_isInitialized) throw new GpioException("GpioChannel has not been set up yet. Invoke 'Setup' before attempting to read/write value");
                if(Direction != EGpioDirection.Input) throw new GpioException(string.Format("Reading gpio{0} failed. This channel is not set as input",(int)Channel));
#if DEBUG
                Console.WriteLine("Reading value of gpio{0}", Channel);
#endif
                lock (_chanLock)
                {
                    using (
                        var fileStream = new FileStream(Path.Combine(GpioDir, "value"), FileMode.Open, FileAccess.Read,
                            FileShare.ReadWrite))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            var result = streamReader.ReadToEnd().Trim() == "1";
#if DEBUG
                            Console.WriteLine("gpio{0} = {1}", Channel,result);
#endif
                            return result;
                        }
                    }
                }
                
            }
            set
            {
                if (!_isInitialized) throw new GpioException("GpioChannel has not been set up yet. Invoke 'Setup' before attempting to read/write value");
                if (Direction != EGpioDirection.Output) throw new GpioException(string.Format("Writing gpio{0} failed. This channel is not set as output", (int)Channel));
#if DEBUG
                Console.WriteLine("Writing value {0} to gpio{1}",value, Channel);
#endif
                lock (_chanLock)
                {
                    using (
                        var fileStream = new FileStream(Path.Combine(GpioDir, "value"), FileMode.Truncate,
                            FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(value ? "1" : "0");
                            streamWriter.Flush();
                        }
                    }
                }
            }
        }

        private void Initialize()
        {
            if (!Directory.Exists(GpioDir)) Export();//directory doesnt exist, so we need to export
            SetDirection();
        }

        private void Export()
        {
            lock (_chanLock)
            {
                using (
                    var fileStream = new FileStream(Path.Combine(GpioPath, "export"), FileMode.Truncate,
                        FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write((int) Channel);
                        streamWriter.Flush();
                    }
                }
            }
        }

        private void SetDirection()
        {
            lock (_chanLock)
            {
                using (
                    var fileStream = new FileStream(Path.Combine(GpioDir, "direction"), FileMode.Truncate,
                        FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(Direction == EGpioDirection.Input ? "in" : "out");
                        streamWriter.Flush();
                    }
                }
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
