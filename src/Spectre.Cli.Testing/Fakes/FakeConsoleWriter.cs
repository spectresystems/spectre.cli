using System;
using System.IO;

namespace Spectre.Cli.Testing.Fakes
{
    public sealed class FakeConsoleWriter : IConsoleWriter
    {
        public StringWriter Output { get; set; }

        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public FakeConsoleWriter()
        {
            Output = new StringWriter();
        }

        public void Write(string text)
        {
            Output.Write(text);
        }

        public override string ToString()
        {
            return Output.ToString()
                .NormalizeLineEndings()
                .Trim();
        }
    }
}
