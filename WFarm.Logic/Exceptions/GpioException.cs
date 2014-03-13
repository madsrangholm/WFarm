using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFarm.Logic.Exceptions
{
    public class GpioException : Exception
    {
        public GpioException(string message) : base(message)
        {
        }

        public GpioException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
