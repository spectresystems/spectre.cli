using System;
using System.ComponentModel;
using Spectre.CommandLine.Internal.Mappings;

namespace Spectre.CommandLine.Internal
{
    internal sealed class Mapper
    {
        public void Map(object settings, Mapping mapping)
        {
            if (mapping.Required && !mapping.HasValue)
            {
                switch (mapping)
                {
                    case OptionMapping option:
                        throw new CommandLineException($"Option {option.Name} is missing.");
                    case ArgumentMapping argument:
                        throw new CommandLineException($"Argument {argument.Name} is missing.");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!mapping.HasValue)
            {
                return;
            }

            // Flag mapping?
            if (mapping.Type == MappingType.Flag)
            {
                mapping.Property.SetValue(settings, mapping.Value == "on");
            }

            // Scalar mapping?
            if (mapping.Type == MappingType.Scalar)
            {
                var converter = TypeDescriptor.GetConverter(mapping.Property.PropertyType);
                var value = converter.ConvertFromInvariantString(mapping.Value);
                mapping.Property.SetValue(settings, value);
            }
        }
    }
}
