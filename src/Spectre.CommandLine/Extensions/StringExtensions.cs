// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    internal static class StringExtensions
    {
        public static bool IsRequiredOrOptionalArgument(this string text)
        {
            if (text != null)
            {
                return (text.StartsWith("[") && text.EndsWith("]")) ||
                       (text.StartsWith("<") && text.EndsWith(">"));
            }
            return false;
        }

        public static bool IsRequiredArgument(this string text)
        {
            if (text != null)
            {
                return text.StartsWith("<") && text.EndsWith(">");
            }
            return false;
        }

        public static string TrimArgument(this string text)
        {
            return text?.TrimStart('[', '<')?.TrimEnd(']', '>');
        }
    }
}