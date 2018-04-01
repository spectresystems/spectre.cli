using System;
using System.Text;
using Spectre.CommandLine.Internal.Rendering;

namespace Spectre.CommandLine.Tests.Utilities
{
    internal sealed class StringRenderer : IRenderer
    {
        private readonly StringBuilder _builder;

        public StringRenderer(StringBuilder builder)
        {
            _builder = builder;
        }

        private sealed class Scope : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public IDisposable SetBackground(ConsoleColor color)
        {
            return new Scope();
        }

        public IDisposable SetForeground(ConsoleColor color)
        {
            return new Scope();
        }

        public void Append(string text)
        {
            _builder.Append(text);
        }
    }
}
