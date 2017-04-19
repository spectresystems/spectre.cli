using System;

namespace Spectre.CommandLine
{
    public sealed class CommandLineException : Exception
    {
        public CommandLineException(string message) 
            : base(message)
        {
        }
    }
}
