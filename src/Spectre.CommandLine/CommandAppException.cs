using System;
using System.Runtime.Serialization;

namespace Spectre.CommandLine
{
    [Serializable]
    public sealed class CommandAppException : Exception
    {
        public CommandAppException()
        {
        }

        public CommandAppException(string message)
            : base(message)
        {
        }

        public CommandAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private CommandAppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}