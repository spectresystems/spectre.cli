using System;

namespace Spectre.Cli.Internal
{
    /// <summary>
    /// Represents a console renderer.
    /// </summary>
    internal interface IRenderer
    {
        /// <summary>
        /// Sets the background color.
        /// </summary>
        /// <param name="color">The color to use.</param>
        /// <returns>A disposable scope that resets the background color.</returns>
        IDisposable SetBackground(ConsoleColor color);

        /// <summary>
        /// Sets the foreground color.
        /// </summary>
        /// <param name="color">The color to use.</param>
        /// <returns>A disposable scope that resets the foreground color.</returns>
        IDisposable SetForeground(ConsoleColor color);

        /// <summary>
        /// Appends text to the renderer.
        /// </summary>
        /// <param name="text">The text to append.</param>
        void Append(string text);
    }
}