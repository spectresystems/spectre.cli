using System;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents the standard output.
    /// </summary>
    public interface IConsoleWriter
    {
        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="text">The value to write.</param>
        void Write(string text);
    }
}
