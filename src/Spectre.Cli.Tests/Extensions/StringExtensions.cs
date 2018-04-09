using System;

// ReSharper disable once CheckNamespace
namespace Spectre.Cli.Tests
{
    internal static class StringExtensions
    {
        public static string NormalizeLineEndings(this string value)
        {
            if (value != null)
            {
                value = value.Replace("\r\n", "\n", StringComparison.Ordinal);
                value = value.Replace("\r", string.Empty, StringComparison.Ordinal);
                return value.Replace("\n", "\r\n", StringComparison.Ordinal);
            }
            return string.Empty;
        }
    }
}
