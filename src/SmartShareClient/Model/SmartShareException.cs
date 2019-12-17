using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShareClient.Model
{
    public class SmartShareException : Exception
    {
        public SmartShareException()
        {
        }

        public SmartShareException(string message) : base(message)
        {
        }

        public SmartShareException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
