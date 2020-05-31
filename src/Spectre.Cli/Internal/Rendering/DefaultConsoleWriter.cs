using System;

namespace Spectre.Cli.Internal
{
    internal sealed class DefaultConsoleWriter : IConsoleWriter
    {
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}
