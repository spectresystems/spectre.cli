namespace Spectre.Cli.Internal
{
    internal static class StringExtensions
    {
        public static string SafeMarkup(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text.Replace("[", "[[");
        }
    }
}
