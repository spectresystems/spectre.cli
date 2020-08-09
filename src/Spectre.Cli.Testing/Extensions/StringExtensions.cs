using System.Collections.Generic;

namespace Spectre.Cli.Testing
{
    public static class StringExtensions
    {
        public static string TrimLines(this string value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var result = new List<string>();
            var lines = value.Split(new[] { '\n' });

            foreach (var line in lines)
            {
                var current = line.TrimEnd();
                if (string.IsNullOrWhiteSpace(current))
                {
                    result.Add(string.Empty);
                }
                else
                {
                    result.Add(current);
                }
            }

            return string.Join("\n", result);
        }

        public static string NormalizeLineEndings(this string value)
        {
            if (value != null)
            {
                value = value.Replace("\r\n", "\n");
                return value.Replace("\r", string.Empty);
            }

            return string.Empty;
        }
    }
}
