using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal sealed class OptionMappingFactory : MappingFactory<OptionDefinition>
    {
        public override IMapping CreateMapping(CommandLineApplication app, OptionDefinition item)
        {
            // Inherited?
            if (item.Inherited)
            {
                return null;
            }

            // Is this even an option?
            var attribute = item.Property.GetCustomAttribute<OptionAttribute>();
            if (attribute == null)
            {
                return null;
            }

            // What kind of property is it?
            var isFlag = item.Property.PropertyType == typeof(bool);
            var type = isFlag ? MappingType.Flag : MappingType.Scalar;

            // Got an description?
            var description = item.Property.GetCustomAttribute<DescriptionAttribute>()?.Description;

            // Get the option type.
            var optionType = type == MappingType.Flag ? CommandOptionType.NoValue : CommandOptionType.SingleValue;

            // Create the option.
            var option = app.Option(attribute.Template, description, optionType, item.Inherited);
            var mapping = new OptionMapping(item.Property, type, option);

            // Got default value?
            var @default = item.Property.GetCustomAttribute<DefaultValueAttribute>();
            if (@default != null)
            {
                // TODO: Validate
                mapping.SetDefaultValue(@default.Value);
            }

            return mapping;
        }
    }
}
