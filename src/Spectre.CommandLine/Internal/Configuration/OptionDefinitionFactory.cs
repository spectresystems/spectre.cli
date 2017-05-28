using System;
using System.Collections.Generic;
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

                    var attribute = property.GetCustomAttribute<OptionAttribute>();
                    if (attribute == null)
                    {
                        continue;
                    }

                    result.Add(new OptionDefinition
                    {
                        Type = property.PropertyType,
                        Property = property,
                        Template = attribute.Template,
                        Inherited = IsInherited(command, settingsType, property, attribute)
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
