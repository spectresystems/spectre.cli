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
        /// Gets the name of the command.
        /// </summary>
        /// <value>
        /// The name of the command.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the data that was passed to the command during registration (if any).
        /// </summary>
        /// <value>
        /// The command data.
        /// </value>
        public object? Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        /// <param name="remaining">The remaining arguments.</param>
        /// <param name="name">The command name.</param>
        /// <param name="data">The command data.</param>
        internal CommandContext(IRemainingArguments remaining, string name, object? data)
        {
            Remaining = remaining;
            Name = name;
            Data = data;
        }
    }
}
