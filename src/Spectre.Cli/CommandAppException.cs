using System;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents errors that occur during application execution.
    /// </summary>
    public abstract class CommandAppException : Exception
    {
        internal IRenderable? Pretty { get; }

        internal virtual bool AlwaysPropagateWhenDebugging => false;

        internal CommandAppException(string message, IRenderable? pretty = null)
            : base(message)
        {
            Pretty = pretty;
        }

        internal CommandAppException(string message, Exception ex, IRenderable? pretty = null)
            : base(message, ex)
        {
            Pretty = pretty;
        }

        /// <summary>
        /// Renders the exception using the specified <see cref="IAnsiConsole"/>.
        /// </summary>
        /// <param name="console">The console.</param>
        public void Render(IAnsiConsole console)
        {
            if (Pretty != null)
            {
                console?.Render(Pretty);
            }
        }
    }
}