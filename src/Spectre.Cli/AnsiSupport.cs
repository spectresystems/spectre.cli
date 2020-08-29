using System;
using SpectreAnsiSupport = Spectre.Console.AnsiSupport;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents ANSI support.
    /// </summary>
    public enum AnsiSupport
    {
        /// <summary>
        /// ANSI escape sequence support should be detected by the system.
        /// </summary>
        Detect = 0,

        /// <summary>
        /// ANSI escape sequences are supported.
        /// </summary>
        Yes = 1,

        /// <summary>
        /// ANSI escape sequences are not supported.
        /// </summary>
        No = 2,
    }

    internal static class AnsiSupportExtensions
    {
        public static SpectreAnsiSupport GetAnsiSupport(this AnsiSupport support)
        {
            return support switch
            {
                AnsiSupport.Detect => SpectreAnsiSupport.Detect,
                AnsiSupport.Yes => SpectreAnsiSupport.Yes,
                AnsiSupport.No => SpectreAnsiSupport.No,
                _ => throw new InvalidOperationException("Unknown ANSI support setting."),
            };
        }
    }
}
