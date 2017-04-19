using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using Spectre.CommandLine.Internal.Mappings;

namespace Spectre.CommandLine.Internal
{
    internal static class MappingFactory
    {
        public static readonly OptionMappingFactory Option;
        public static readonly ArgumentMappingFactory Argument;

        static MappingFactory()
        {
            Option = new OptionMappingFactory();
            Argument = new ArgumentMappingFactory();
        }

        public static IEnumerable<Mapping> CreateMappings(CommandLineApplication app, Type settingsType)
        {
            var options = Option.CreateMappings(app, settingsType);
            var arguments = Argument.CreateMappings(app, settingsType);
            return options.Concat(arguments).ToArray();
        }
    }

    internal abstract class MappingFactory<T>
        where T : Mapping
    {
        public IEnumerable<Mapping> CreateMappings(CommandLineApplication app, Type settings)
        {
            var properties = settings.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in SortMappings(app, properties))
            {
                var mapping = CreateMapping(app, property);
                if (mapping != null)
                {
                    yield return mapping;
                }
            }
        }

        public Mapping CreateMapping(CommandLineApplication app, PropertyInfo property)
        {
            // Get the mapping type.
            var isFlag = property.PropertyType == typeof(bool);
            var type = isFlag ? MappingType.Flag : MappingType.Scalar;

            // Create mapping.
            var result = CreateMapping(app, property, type);
            if (result != null)
            {
                // Set common mapping properties.
                result.Required = property.GetCustomAttribute<RequiredAttribute>() != null;
            }

            // Return the result.
            return result;
        }

        protected abstract IEnumerable<PropertyInfo> SortMappings(
            CommandLineApplication app, 
            IEnumerable<PropertyInfo> properties);

        protected abstract T CreateMapping(
            CommandLineApplication app, 
            PropertyInfo property, 
            MappingType type);
    }
}
