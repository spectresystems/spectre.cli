using System;
using Spectre.CommandLine.Internal.Rendering;
using Spectre.CommandLine.Internal.Rendering.Elements;

namespace Spectre.CommandLine
{
    /// <summary>
    /// Represents errors that occur during application execution.
    /// </summary>
    /// <seealso cref="Exception" />
    /// <seealso cref="IRenderable" />
    public class CommandAppException : Exception, IRenderable
    {
        internal IRenderable Pretty { get; }

        /// <summary>
        /// Gets a value indicating whether this exception always should
        /// propagate if there is a debugger attached.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this exception always should
        ///   propagate if there is a debugger attached; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AlwaysPropagateWhenDebugging => false;

        internal CommandAppException(string message, IRenderable pretty = null)
            : base(message)
        {
            Pretty = pretty;
        }

        internal CommandAppException(string message, Exception ex, IRenderable pretty = null)
            : base(message, ex)
        {
            Pretty = pretty;
        }

        void IRenderable.Render(IRenderer renderer)
        {
            var pretty = Pretty ?? new TextElement(Message ?? "An unknown configuration exception occured.");
            pretty.Render(renderer);
        }
    }
}