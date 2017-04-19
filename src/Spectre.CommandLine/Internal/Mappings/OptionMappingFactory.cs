using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace Spectre.CommandLine.Internal.Mappings
{
    internal sealed class OptionMappingFactory : MappingFactory<OptionMapping>
    {
        protected override IEnumerable<PropertyInfo> SortMappings(
            CommandLineApplication app, IEnumerable<PropertyInfo> properties)
        {
            return properties;
        }

        protected override OptionMapping CreateMapping(
            CommandLineApplication app, PropertyInfo property, MappingType type)
        {
            // Is this even an option?
            var attribute = property.GetCustomAttribute<OptionAttribute>();
            if (attribute == null)
            {
                return null;
            }

            // Got a description?
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            var description = descriptionAttribute?.Description ?? string.Empty;

            // Create the option.
            var template = attribute.Template;
            var optionType = type == MappingType.Flag ? CommandOptionType.NoValue : CommandOptionType.SingleValue;
            var option = app.Option(template, description, optionType);

            // Create a mapping.
            return new OptionMapping(property, type, option);
        }
    }
}