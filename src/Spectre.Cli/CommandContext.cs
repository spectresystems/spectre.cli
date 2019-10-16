using System.Collections.Generic;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents a command context.
    /// </summary>
    public sealed class CommandContext
    {
        /// <summary>
        /// Gets the remaining arguments.
        /// </summary>
        /// <value>
        /// The remaining arguments.
        /// </value>
        public IRemainingArguments Remaining { get; }

        /// <summary>
        /// Gets the data that was passed to the command during registration (if any).
        /// </summary>
        /// <value>
        /// The command data.
        /// </value>
        public object Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="remaining">The remaining arguments.</param>
        /// <param name="data">The command data.</param>
        internal CommandContext(IRemainingArguments remaining, object data)
        {
            Remaining = remaining;
            Data = data;
        }
    }
}
