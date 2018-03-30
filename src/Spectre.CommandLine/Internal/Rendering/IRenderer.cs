using System;

namespace Spectre.CommandLine.Internal.Rendering
{
    internal interface IRenderer
    {
        IDisposable SetBackground(ConsoleColor color);
        IDisposable SetForeground(ConsoleColor color);

        void Append(string text);
    }
}