using System;
using System.IO;
using WFarm.Logic.Enums;
using WFarm.Logic.Exceptions;
using WFarm.Logic.Interfaces;

namespace WFarm.Hardware.Gpio
{
    public class GpioChannel : IGpioChannel
    {
        private static readonly object FileSystemLock = new object();

        private static string GpioPath
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.WinCE:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                        return @"C:\RasPiGpioTest";
                    case PlatformID.MacOSX:
                        return @"/tmp/RasPiGpioTest";
                    default:
                        return @"/sys/class/gpio";
                }
            }
        }

        

        public EGpioDirection Direction { get; set; }

        public EGpioChannel Channel { get; set; }
        private string GpioDir
        {
            get { return Path.Combine(GpioPath, string.Format("gpio{0}", (int) Channel)); }
        }

        public void Setup(EGpioChannel channel, EGpioDirection direction)
        {
            Channel = channel;
            Direction = direction;
            Initialize();
        }
        public bool Value
        {
            get
            {
                if(Direction != EGpioDirection.Input) throw new GpioException(string.Format("Reading gpio{0} failed. This channel is not set as input",(int)Channel));
                lock (FileSystemLock)
                {
                    using (
                        var fileStream = new FileStream(Path.Combine(GpioDir, "value"), FileMode.Open, FileAccess.Read,
                            FileShare.ReadWrite))
                    {
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            return streamReader.ReadToEnd().Trim() == "1";
                        }
                    }
                }
            }
            set
            {
                if (Direction != EGpioDirection.Output) throw new GpioException(string.Format("Writing gpio{0} failed. This channel is not set as output", (int)Channel));
                lock (FileSystemLock)
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
            lock (FileSystemLock)
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
            lock (FileSystemLock)
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
