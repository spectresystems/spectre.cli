using System;
using System.IO;

namespace Spectre.CommandLine
{
    public interface IConsoleStreams : IDisposable
    {
        TextWriter Out { get; }
        TextWriter Error { get; }
    }
}