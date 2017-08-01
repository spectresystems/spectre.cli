using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (source is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    source.Add(item);
                }
            }
        }
    }
}
