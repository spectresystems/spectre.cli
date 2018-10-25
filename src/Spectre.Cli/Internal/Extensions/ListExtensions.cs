using System.Collections.Generic;

namespace Spectre.Cli.Internal
{
    internal static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
