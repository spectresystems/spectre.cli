using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Spectre.CommandLine.Internal
{
    internal static class OptionDefinitionFactory
    {
        public static IEnumerable<OptionDefinition> CreateOptions(CommandDefinition command, Type settingsType)
        {
            var result = new List<OptionDefinition>();
            if (settingsType != null)
            {
                var properties = settingsType.GetTypeInfo().GetProperties();
                foreach (var property in properties)
                {
                    if (property.SetMethod == null)
                    {
                        continue;
                    }

                    // Make sure it's an option.
                    var attribute = property.GetCustomAttribute<OptionAttribute>();
                    if (attribute == null)
                    {
                        continue;
                    }

                    // Get the default attribute if present.
                    var defaultValue = property.GetCustomAttribute<DefaultValueAttribute>();

                    // What kind of property is it?
                    var isFlag = property.PropertyType == typeof(bool);
                    var mappingType = isFlag ? MappingType.Flag : MappingType.Scalar;

                    result.Add(new OptionDefinition
                    {
                        Type = property.PropertyType,
                        Property = property,
                        Template = attribute.Template,
                        Inherited = IsInherited(command, settingsType, property, attribute),
                        Description = property.GetCustomAttribute<DescriptionAttribute>()?.Description,
                        MappingType = mappingType,
                        HasDefaultValue = defaultValue != null,
                        DefaultValue = defaultValue?.Value
                    });
                }
            }
            return result;
        }

        private static bool IsInherited(CommandDefinition command, Type settingsType, MemberInfo property, OptionAttribute attribute)
        {
            if (property.DeclaringType != settingsType)
            {
                // An option is only considered inherited if it exists
                // higher up in the command hierarchy.

                var current = command.Parent;
                while (current != null)
                {
                    if (current.Options.Any(x => x.Template == attribute.Template))
                    {
                        return true;
                    }
                    current = current.Parent;
                }
            }
            return false;
        }
    }
}
