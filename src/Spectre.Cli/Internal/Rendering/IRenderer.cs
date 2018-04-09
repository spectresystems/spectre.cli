using System;

namespace Spectre.Cli.Internal.Rendering
{
    internal interface IRenderer
    {
        IDisposable SetBackground(ConsoleColor color);
        IDisposable SetForeground(ConsoleColor color);

        void Append(string text);
    }
}