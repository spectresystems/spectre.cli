using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spectre.Cli.Testing.Fakes
{
    public sealed class FakeConsole : IDisposable, IConsoleSettings
    {
        public Encoding Encoding { get; }

        public StringWriter Writer { get; }
        public string Output => Writer.ToString().TrimEnd('\n');
        public IReadOnlyList<string> Lines => Output.Split(new char[] { '\n' });

        public AnsiSupport Ansi => AnsiSupport.No;
        public ColorSupport Colors => ColorSupport.EightBit;
        public TextWriter Out => Writer;

        public FakeConsole()
        {
            Writer = new StringWriter();
        }

        public void Dispose()
        {
            Writer.Dispose();
        }

        public void Write(string text)
        {
            Writer.Write(text);
        }
    }
}
