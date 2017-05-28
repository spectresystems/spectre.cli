using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal static class MappingFactory
    {
        public static readonly OptionMappingFactory Option;

        static MappingFactory()
        {
            Option = new OptionMappingFactory();
        }

        public static MappingCollection CreateMappings(CommandLineApplication app, MappingCollection parent, CommandDefinition command)
        {
            return Option.CreateMappings(app, parent, command.Options);
        }
    }

    internal abstract class MappingFactory<TSource>
    {
        public MappingCollection CreateMappings(CommandLineApplication app, MappingCollection parent, IEnumerable<TSource> items)
        {
            var result = new MappingCollection(parent);
            foreach (var property in SortMappings(app, items))
            {
                var mapping = CreateMapping(app, property);
                if (mapping != null)
                {
                    result.Add(mapping);
                }
            }
            return result;
        }

        public abstract IMapping CreateMapping(CommandLineApplication app, TSource item);

        protected virtual IEnumerable<TSource> SortMappings(
            CommandLineApplication app,
            IEnumerable<TSource> items)
        {
            return items;
        }
    }
}