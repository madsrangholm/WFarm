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
                Console.WriteLine("Reading value of gpio{0}", Channel);
                lock (_chanLock)
                {
                    return ReadFile(Path.Combine(GpioDir, "value"));
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
                    Writefile(Path.Combine(GpioDir, "value"), value ? "1" : "0");
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

                if (_config.SystemPlatform == ESystemPlatform.Windows) //create folders manually
                {
                    if (!Directory.Exists(GpioDir))
                    {
                        Directory.CreateDirectory(GpioDir);
                        File.Create(Path.Combine(GpioDir, "direction")).Close();
                        Writefile(Path.Combine(GpioDir, "direction"),"in");
                        File.Create(Path.Combine(GpioDir, "value")).Close();
                        Writefile(Path.Combine(GpioDir, "value"), "0");
                    }
                }
                else
                {
                    Writefile(Path.Combine(GpioPath, "export"), ((int) Channel).ToString());
                }
            }
        }

        private void SetDirection()
        {
            lock (_chanLock)
            {
                Writefile(Path.Combine(GpioDir, "direction"), Direction == EGpioDirection.Input ? "in" : "out");
            }
        }

        private bool ReadFile(string path)
        {
            using (var fileStream = new FileStream(Path.Combine(path,""), 
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var result = streamReader.ReadToEnd().Trim() == "1";
                    Console.WriteLine("gpio{0} = {1}", Channel, result);
                    return result;
                }
            }
        }

        private void Writefile(string path, string value)
        {
            using (var fileStream = new FileStream(Path.Combine(path, ""), 
                        FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(value);
                    streamWriter.Flush();
                }
            }
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
