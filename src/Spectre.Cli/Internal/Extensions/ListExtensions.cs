using System.Collections.Generic;

namespace Spectre.Cli.Internal
{
    internal static class ListExtensions
    {
        public static T AddAndReturn<T>(this IList<T> source, T item)
            where T : class
        {
            source.Add(item);
            return item;
        }

        public static void AddIfNotNull<T>(this IList<T> source, T? item)
            where T : class
        {
            if (item != null)
            {
                source.Add(item);
            }
        }

        public static void AddRangeIfNotNull<T>(this IList<T> source, IEnumerable<T?> items)
            where T : class
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    source.Add(item);
                }
            }
        }

        public static void AddRange<T>(this IList<T> source, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}
