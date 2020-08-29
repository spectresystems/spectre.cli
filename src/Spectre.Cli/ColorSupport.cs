using System;
using Spectre.Console;

namespace Spectre.Cli
{
    /// <summary>
    /// Represents different color systems.
    /// </summary>
    public enum ColorSupport
    {
        /// <summary>
        /// Try to detect the color system.
        /// </summary>
        Detect,

        /// <summary>
        /// Legacy, 3-bit mode.
        /// </summary>
        ThreeBit,

        /// <summary>
        /// Standard, 4-bit mode.
        /// </summary>
        FourBit,

        /// <summary>
        /// 8-bit mode.
        /// </summary>
        EightBit,

        /// <summary>
        /// 24-bit mode.
        /// </summary>
        TrueColor,
    }

    internal static class ColorSupportExtensions
    {
        public static ColorSystemSupport GetColorSystem(this ColorSupport support)
        {
            return support switch
            {
                ColorSupport.Detect => ColorSystemSupport.Detect,
                ColorSupport.ThreeBit => ColorSystemSupport.Legacy,
                ColorSupport.FourBit => ColorSystemSupport.Standard,
                ColorSupport.EightBit => ColorSystemSupport.EightBit,
                ColorSupport.TrueColor => ColorSystemSupport.TrueColor,
                _ => throw new InvalidOperationException("Unknown color support setting."),
            };
        }
    }
}
