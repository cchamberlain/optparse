using System;

namespace OptParse
{
    internal class OptParseException : Exception
    {
        internal OptParseException(string message) : base(message)
        {}

        internal OptParseException(string message, Exception innerException) : base(message, innerException)
        {}
    }
}
