using System;
using System.IO;

namespace Spectre.CommandLine
{
    internal sealed class ConsoleStreams : IDisposable
    {
        public TextWriter Out { get; }
        public TextWriter Error { get; }

        public ConsoleStreams()
        {
            Out = new StringWriter();
            Error = new StringWriter();
        }

        public void Dispose()
        {
            Out?.Dispose();
            Error?.Dispose();
        }
    }
}
