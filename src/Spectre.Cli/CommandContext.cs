using System.Collections.Generic;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command context.
    /// </summary>
    public sealed class CommandContext
    {
        /// <summary>
        /// Gets a list of the remaining arguments.
        /// </summary>
        /// <value>
        /// The remaining arguments.
        /// </value>
        public IReadOnlyList<string> Remaining { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="remaining">The remaining arguments.</param>
        internal CommandContext(IReadOnlyList<string> remaining)
        {
            Remaining = remaining;
        }
    }
}
