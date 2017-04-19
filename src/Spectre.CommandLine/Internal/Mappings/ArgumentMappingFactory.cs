using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Spectre.CommandLine.Internal.Mappings
{
    internal sealed class ArgumentMappingFactory : MappingFactory<ArgumentMapping>
    {
        protected override IEnumerable<PropertyInfo> SortMappings(
            CommandLineApplication app, IEnumerable<PropertyInfo> properties)
        {
            var result = new List<Tuple<int, PropertyInfo>>();
            foreach (var property in properties)
            {
                // Is this even an argument?
                var attribute = property.GetCustomAttribute<ArgumentAttribute>();
                if (attribute != null)
                {
                    result.Add(Tuple.Create(attribute.Order, property));
                }
            }
            return result.OrderBy(i => i.Item1).Select(i => i.Item2);
        }

        protected override ArgumentMapping CreateMapping(
            CommandLineApplication app, PropertyInfo property, MappingType type)
        {
            // Is this even an argument?
            var attribute = property.GetCustomAttribute<ArgumentAttribute>();
            if (attribute == null)
            {
                return null;
            }

            // Got a description?
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            var description = descriptionAttribute?.Description ?? string.Empty;

            // Create the mapping.
            var argument = app.Argument(attribute.Name, description);
            argument.ShowInHelpText = true;
            return new ArgumentMapping(property, type, argument);
        }
    }
}
