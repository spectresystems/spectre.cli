using System;

namespace Spectre.Cli.Internal
{
    internal interface IRenderer
    {
        IDisposable SetBackground(ConsoleColor color);
        IDisposable SetForeground(ConsoleColor color);

        void Append(string text);
    }
}