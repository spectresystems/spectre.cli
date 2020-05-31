using System;
using Spectre.Cli.Internal;

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
        /// Renders the exception using the specified <see cref="IConsoleWriter"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Render(IConsoleWriter writer)
        {
            Pretty?.Render(new ConsoleRenderer(writer));
        }
    }
}