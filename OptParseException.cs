using System;

namespace OptParse
{
    public class OptParseException : Exception
    {
        public OptParseException(string message) : base(message)
        {}

        public OptParseException(string message, Exception innerException) : base(message, innerException)
        {}
    }
}
