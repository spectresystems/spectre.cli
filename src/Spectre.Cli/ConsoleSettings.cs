using System;
using System.IO;
using Spectre.Console;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents console settings.
    /// </summary>
    public interface IConsoleSettings
    {
        /// <summary>
        /// Gets a value indicating whether or not ANSI escape sequences are supported.
        /// </summary>
        public AnsiSupport Ansi { get; }

        /// <summary>
        /// Gets what color system that is used.
        /// </summary>
        public ColorSupport Colors { get; }

        /// <summary>
        /// Gets the out buffer.
        /// </summary>
        public TextWriter? Out { get; }
    }

    /// <summary>
    /// The console settings.
    /// </summary>
    public sealed class ConsoleSettings : IConsoleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not ANSI escape sequences are supported.
        /// </summary>
        public AnsiSupport Ansi { get; set; }

        /// <summary>
        /// Gets or sets what color system that is used.
        /// </summary>
        public ColorSupport Colors { get; set; }

        /// <summary>
        /// Gets or sets the out buffer.
        /// </summary>
        public TextWriter? Out { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleSettings"/> class.
        /// </summary>
        public ConsoleSettings()
        {
            Ansi = AnsiSupport.Detect;
            Colors = ColorSupport.Detect;
        }
    }

    internal static class ConsoleSettingsExtensions
    {
        public static IAnsiConsole CreateConsole(this IConsoleSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = settings.Out,
                Ansi = settings.Ansi.GetAnsiSupport(),
                ColorSystem = settings.Colors.GetColorSystem(),
            });
        }
    }
}
