namespace Spectre.CommandLine
{
    internal static class StringExtensions
    {
        public static bool IsRequiredOrOptionalArgument(this string text)
        {
            if (text != null)
            {
                return text.StartsWith("[") && text.EndsWith("]") &&
                       text.StartsWith("<") && text.EndsWith(">");
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